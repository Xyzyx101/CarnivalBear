using UnityEngine;
using System.Collections;

public class RollerBomb : MonoBehaviour
{
    private Vector3 PrevPos;
    public float RotateSpeed;
    public Transform Wheel;

    public float AttackTime;
    private float AttackTimer;

    public GameObject ExplosionPrefab;

    private Vector3 AttackDirection;
    private Transform Bear;

    NavMeshAgent Agent;
    Rigidbody RB;
    CapsuleCollider Capsule;

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
        RB = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        CurrentMode = Mode.Approach;
        PrevPos = transform.position;
        Bear = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void Update()
    {
        // Wheel rotation
        float vel = (PrevPos - transform.position).magnitude;
        PrevPos = transform.position;
        Quaternion wheelRotation = Quaternion.AngleAxis(vel * RotateSpeed * Time.deltaTime, Vector3.right);
        Wheel.transform.localRotation *= wheelRotation;

        switch (CurrentMode)
        {
            case Mode.Approach:
                Agent.destination = Bear.position;
                break;
            case Mode.Attack:
                AttackTimer -= Time.deltaTime;
                if (AttackTimer < 0.0f)
                {
                    CurrentMode = Mode.Explode;
                }
                break;
            case Mode.Explode:
                Instantiate(ExplosionPrefab, transform.position, transform.rotation);
                Destroy(gameObject, 0.15f);
                CurrentMode = Mode.Dead;
                break;
            case Mode.Dead:
                break;
        }
    }

    void FixedUpdate()
    {
        if (CurrentMode == Mode.Attack)
        {
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero;
            Vector3 direction = (Bear.position - transform.position).normalized;
            direction.y = -1f;
            Quaternion lookRotation = Quaternion.LookRotation(direction);
            RB.MoveRotation(Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 0.5f));
            RB.MovePosition(transform.position + transform.forward * Time.fixedDeltaTime * 8.0f);
        }
        else
        {
            RB.velocity = Vector3.zero;
            RB.angularVelocity = Vector3.zero;
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            CurrentMode = Mode.Attack;
            AttackTimer = AttackTime;
            AttackDirection = other.transform.position - transform.position;
            AttackDirection.y = 0.0f;
            Agent.enabled = false;
            Capsule.enabled = true;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Player")
        {
            AttackTimer = 0.0f;
        }
    }
}
