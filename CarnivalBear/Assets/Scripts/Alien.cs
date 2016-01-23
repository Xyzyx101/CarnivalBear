using UnityEngine;
using System.Collections;

public class Alien : Enemy
{
    [SerializeField]
    private float ReadyShootTime = 2f;
    private float ReadyShootTimer;

    [SerializeField]
    private float ShootTime = 3f;
    private float ShootTimer;

    [SerializeField]
    private float DieTime = 5f;
    private float DieTimer;

    [SerializeField]
    GameObject ProjectilePrefab;
    [SerializeField]
    GameObject MuzzelFlash;
    [SerializeField]
    Transform ProjectileSpawn;
    [SerializeField]
    Transform PlayerTransform;

    private AudioSource GunAudio;

    enum State
    {
        Run,
        ReadyToShoot,
        Shoot,
        Die
    }
    private State CurrentState;
    private PlayerCharacter Player;
    private Vector3 RunTarget;
    private Vector3 PrevPosition;

    private NavMeshAgent Agent;
    private Animator MyAnimator;
    private AlienAnimHashIDs AnimHash;

    void Awake()
    {
        MyAnimator = GetComponentInChildren<Animator>();
        AnimHash = GetComponent<AlienAnimHashIDs>();
        Agent = GetComponent<NavMeshAgent>();
        PlayerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        Player = PlayerTransform.GetComponent<PlayerCharacter>();
        CurrentState = State.Run;
        DieTimer = DieTime;
        MuzzelFlash.SetActive(false);
        GunAudio = GetComponent<AudioSource>();
    }

    void Update()
    {
        switch (CurrentState)
        {
            case State.Run:
                if (RunTarget == Vector3.zero)
                {
                    RunTarget = GetNewRunTarget();
                    Agent.SetDestination(RunTarget);
                }
                if ((RunTarget - transform.position).sqrMagnitude < 25f)
                {
                    CurrentState = State.ReadyToShoot;
                    ReadyShootTimer = ReadyShootTime;
                    MyAnimator.SetTrigger(AnimHash.ShootTrigger);
                }
                break;
            case State.ReadyToShoot:
                Vector3 rotDir = MathUtils.ProjectVectorOnPlane(Vector3.up, Player.transform.position - transform.position).normalized;
                Quaternion rot = Quaternion.LookRotation(rotDir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, rot, 3f * Time.deltaTime);
                ReadyShootTimer -= Time.deltaTime;
                if (ReadyShootTimer < 0f)
                {
                    CurrentState = State.Shoot;
                    ShootTimer = ShootTime;
                    GameObject proj = Instantiate(ProjectilePrefab, ProjectileSpawn.position, ProjectileSpawn.rotation) as GameObject;
                    AlienProjectile alienProj = proj.GetComponent<AlienProjectile>();
                    alienProj.Shoot(Player.GetTargetPoint());
                    GunAudio.Play();
                }
                break;
            case State.Shoot:
                MuzzelFlash.SetActive(true);
                ShootTimer -= Time.deltaTime;
                if (ShootTimer < 0f)
                {
                    MuzzelFlash.SetActive(false);
                    RunTarget = Vector3.zero;
                    CurrentState = State.Run;
                }
                break;
            case State.Die:
                Agent.speed = 0.5f;
                MuzzelFlash.SetActive(false);
                DieTimer -= Time.deltaTime;
                if (DieTimer < 0f)
                {
                    Destroy(gameObject);
                }
                break;
        }
        UpdateAnimator();
    }

    Vector3 GetNewRunTarget()
    {
        Vector3 newTarget = Player.transform.position + Random.insideUnitSphere * 12;
        newTarget.y = Player.transform.position.y;
        return newTarget;
    }

    void UpdateAnimator()
    {
        Vector3 move = (transform.position - PrevPosition).normalized;
        PrevPosition = transform.position;
        Vector3 localMove = transform.InverseTransformDirection(move);
        float turnAmount = Mathf.Atan2(localMove.x, localMove.z);
        float forwardAmount = localMove.z;
        MyAnimator.SetFloat(AnimHash.ForwardFloat, forwardAmount, 0.5f, Time.fixedDeltaTime);
        MyAnimator.SetFloat(AnimHash.TurnFloat, turnAmount, 0.1f, Time.fixedDeltaTime);
    }

    protected override void Die()
    {
        CurrentState = State.Die;
        MyAnimator.SetTrigger(AnimHash.DieTrigger);
    }
}
