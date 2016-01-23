using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Robot : Enemy
{
    public Transform Wheel1;
    public Transform Wheel2;
    public float HeadingRotSpeed;
    public float WheelRotSpeed;

    private Vector3 AttackDirection;
    private Transform PlayerTransform;
    private PlayerCharacter Player;
    private Vector3 PrevPosition;
    private Quaternion NewRot;

    NavMeshAgent Agent;
    Rigidbody RB;

    private float AgentUpdateTimer;
    [SerializeField]
    LayerMask GroundMask;

    [SerializeField]
    GameObject Lightening;
    [SerializeField]
    Transform LighteningTarget;

    [SerializeField]
    float ShockDamageInterval;
    [SerializeField]
    float Damage;
    [SerializeField]
    float ShockStrength;
    private float ShockTimer;
    [SerializeField]
    private GameObject ExplosionPrefab;
    private AudioSource LighteningAudio;

    enum Mode
    {
        Approach,
        Attack,
        Explode,
        Dead
    }
    private Mode CurrentMode;

    void Start()
    {
        Agent = GetComponent<NavMeshAgent>();
        AgentUpdateTimer = 0f;
        RB = GetComponent<Rigidbody>();
        CurrentMode = Mode.Approach;
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Player = PlayerTransform.GetComponent<PlayerCharacter>();
        Agent.destination = PlayerTransform.position;
        PrevPosition = transform.position;
        Lightening.SetActive(false);
        LighteningAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        // Wheel rotation
        float vel = (PrevPosition - transform.position).magnitude;
        Quaternion wheelRotation = Quaternion.AngleAxis(vel * WheelRotSpeed * Time.deltaTime, Vector3.right);
        Wheel1.transform.localRotation *= wheelRotation;
        Wheel2.transform.localRotation *= wheelRotation;

        AgentUpdateTimer -= Time.deltaTime;
        if (AgentUpdateTimer < 0f)
        {
            Agent.SetDestination(PlayerTransform.position);
            AgentUpdateTimer = 0.2f;
            Vector3 groundNormal = Vector3.up;
            RaycastHit hit;
            Vector3 origin = transform.position + transform.up * 0.35f;
            Vector3 direction = -Vector3.up;
            float maxDistance = 100f;
            if (Physics.Raycast(origin, direction, out hit, maxDistance, GroundMask))
            {
                groundNormal = hit.normal;
            }
            Vector3 moveDir;
            if ((PlayerTransform.position - transform.position).sqrMagnitude > 5f)
            {
                moveDir = transform.position - PrevPosition;
            }
            else
            {
                moveDir = PlayerTransform.position - transform.position;
            }

            PrevPosition = transform.position;
            Vector3 rotDir = MathUtils.ProjectVectorOnPlane(groundNormal, moveDir).normalized;
            NewRot = Quaternion.LookRotation(rotDir, Vector3.up);
        }
        transform.rotation = Quaternion.Slerp(transform.rotation, NewRot, HeadingRotSpeed * Time.deltaTime);

        if (Lightening.activeSelf)
        {
            LighteningTarget.position = Player.GetTargetPoint();
            ShockTimer -= Time.deltaTime;
            if (ShockTimer < 0f)
            {
                ShockTimer = ShockDamageInterval;
                Player.Shock(Damage, ShockStrength);
            }
        }
    }

    public void ShockTarget()
    {
        Lightening.SetActive(true);
        ShockTimer = ShockDamageInterval;
        if (!LighteningAudio.isPlaying)
        {
            LighteningAudio.Play();
        }
    }

    protected override void Die()
    {
        Lightening.SetActive(false);
        LighteningAudio.Stop();
        var colliders = Physics.OverlapSphere(transform.position, 4f);
        var rigidbodies = new List<Rigidbody>();
        foreach (var col in colliders)
        {
            if (col.attachedRigidbody != null && !rigidbodies.Contains(col.attachedRigidbody))
            {
                rigidbodies.Add(col.attachedRigidbody);
            }
        }
        foreach (var rb in rigidbodies)
        {
            rb.AddExplosionForce(200f, transform.position, 4f, 2f, ForceMode.Impulse);
        }
        Instantiate(ExplosionPrefab, transform.position + Vector3.up, transform.rotation);
        Destroy(gameObject, 1f);
    }
}
