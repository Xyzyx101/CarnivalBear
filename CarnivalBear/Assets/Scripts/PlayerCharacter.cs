using UnityEngine;
using System.Collections;

enum AttackState
{
    Idle,
    Atk1,
    Atk2,
    Atk3,
    Atk4,
    Atk5,
    Atk6
}

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
public class PlayerCharacter : MonoBehaviour
{
    [SerializeField]
    float MovingTurnSpeed = 360;
    [SerializeField]
    float StationaryTurnSpeed = 180;
    [SerializeField]
    float CrouchingTurnSpeed = 90;
    [SerializeField]
    float GroundCheckDistance = 0.2f;
    [SerializeField]
    float AnimSpeedMultiplier = 1f;
    [SerializeField]
    float JumpPower = 12f;
    [SerializeField]
    float Acceleration = 1f;
    [SerializeField]
    float MaxSpeed = 5f;
    [SerializeField]
    float MaxCrouchSpeed = 1.5f;
    [SerializeField]
    float GravityMultiplier = 2f;
    [SerializeField]
    Vector3 Friction;
    [SerializeField]
    float HP;
    [SerializeField]
    float MaxHP;

    Rigidbody RB;
    Animator MyAnimator;
    CapsuleCollider Capsule;

    bool Grounded;
    float OriginalGroundCheckDistance;
    Vector3 GroundNormal;
    float TurnAmount;
    float ForwardAmount;
    bool Crouching;
    Vector3 CapsuleCenter;
    float CapsuleHeight;
    public Vector3 Velocity;
    private float RunSpeed;

    [SerializeField]
    float ComboTime;
    public bool CanAttack;
    bool ComboActive;
    AttackState CurrentAttackState;
    float ComboTimer;
    int ComboCount;

    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        CapsuleHeight = Capsule.height;
        CapsuleCenter = Capsule.center;
        OriginalGroundCheckDistance = GroundCheckDistance;
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        MyAnimator.applyRootMotion = false;
        CanAttack = true;
        HP = 100f;
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool attack)
    {
        Attack(attack);
        CheckGrounded();
        if (CurrentAttackState == AttackState.Atk3 || CurrentAttackState == AttackState.Atk6)
        {
            move = Vector3.zero;
        }
        move = Vector3.ProjectOnPlane(move, GroundNormal);
        ApplyTurnRotation();
        if (Grounded)
        {
            Velocity = RB.velocity;
            Velocity += move * Acceleration * Time.fixedDeltaTime;
            float originalY = Velocity.y;
            Velocity.y = 0;
            Velocity = Vector3.ClampMagnitude(Velocity, crouch ? MaxCrouchSpeed : MaxSpeed);
            Velocity.y = originalY;
            RB.velocity = Velocity;
            Jump(crouch, jump);
        }
        else
        {
            AirborneMovement();
        }
        ScaleCapsule(crouch);

        Vector3 localMove = transform.InverseTransformDirection(move);
        TurnAmount = Mathf.Atan2(localMove.x, localMove.z);
        ForwardAmount = localMove.z;

        // Directional Friction
        if (Grounded)
        {
            float forwardVelocity = Vector3.Dot(RB.velocity, transform.forward);
            RunSpeed = forwardVelocity;
            float rightVelocity = Vector3.Dot(RB.velocity, transform.right);
            float upVelocity = Vector3.Dot(RB.velocity, transform.up);
            Vector3 friction =
                    forwardVelocity * Friction.z * transform.forward +
                    rightVelocity * Friction.x * transform.right +
                    upVelocity * Friction.z * transform.up;
            // Extra friction when you let go of the controller
            if (forwardVelocity < 3f && localMove.z < 0.25f)
            {
                friction *= 3f;
            }
            // Extra friction when you are attacking
            if (attack)
            {
                friction *= 10f;
            }
            RB.AddForce(-friction, ForceMode.Acceleration);
        }
        UpdateAnimator(localMove);
    }

    void Jump(bool crouch, bool jump)
    {
        if (jump && !crouch && MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            Vector3 jumpForce = Vector3.up * JumpPower * Time.fixedDeltaTime;
            RB.AddForce(jumpForce, ForceMode.VelocityChange);
            Grounded = false;
            GroundCheckDistance = 0.1f;
        }
    }

    void Attack(bool attack)
    {
        if (!CanAttack)
        {
            return;
        }
        else if (!attack && !ComboActive)
        {
            return;
        }
        else if (attack && !ComboActive)
        {
            ComboTimer = ComboTime;
        }
        if (Crouching)
        {
            MyAnimator.SetTrigger("AtkCrouchTrigger");
            return;
        }
        if (!Grounded)
        {
            MyAnimator.SetTrigger("AtkJumpTrigger");
            return;
        }
        ComboActive = true;
        ComboTimer -= Time.fixedDeltaTime;
        if (ComboTimer < 0f)
        {
            CurrentAttackState = AttackState.Idle;
            ComboTimer = ComboTime;
            ComboCount = 0;
            ComboActive = false;
        }
        if (!attack)
        {
            return;
        }
        ComboCount++;
        Debug.Log("ComboCount:" + ComboCount + " State:" + CurrentAttackState);
        float q = Random.value;
        switch (CurrentAttackState)
        {
            case AttackState.Idle:
                if (q < 0.25f)
                {
                    MyAnimator.SetTrigger("Atk1Trigger");
                    CurrentAttackState = AttackState.Atk1;
                }
                else if (q < 0.5f)
                {
                    MyAnimator.SetTrigger("Atk2Trigger");
                    CurrentAttackState = AttackState.Atk2;
                }
                else if (q < 0.75f)
                {
                    MyAnimator.SetTrigger("Atk4Trigger");
                    CurrentAttackState = AttackState.Atk4;
                }
                else
                {
                    MyAnimator.SetTrigger("Atk5Trigger");
                    CurrentAttackState = AttackState.Atk5;
                }
                break;
            case AttackState.Atk1:
            case AttackState.Atk4:
                if (ComboCount == 3)
                {
                    MyAnimator.SetTrigger("Atk6Trigger");
                    CurrentAttackState = AttackState.Atk6;
                }
                else
                {
                    if (q < 0.5f)
                    {
                        MyAnimator.SetTrigger("Atk2Trigger");
                        CurrentAttackState = AttackState.Atk2;
                    }
                    else
                    {
                        MyAnimator.SetTrigger("Atk5Trigger");
                        CurrentAttackState = AttackState.Atk5;
                    }
                }
                break;
            case AttackState.Atk2:
            case AttackState.Atk5:
                if (ComboCount == 3)
                {
                    MyAnimator.SetTrigger("Atk3Trigger");
                    CurrentAttackState = AttackState.Atk6;
                }
                else
                {
                    if (q < 0.5f)
                    {
                        MyAnimator.SetTrigger("Atk1Trigger");
                        CurrentAttackState = AttackState.Atk1;
                    }
                    else
                    {
                        MyAnimator.SetTrigger("Atk4Trigger");
                        CurrentAttackState = AttackState.Atk4;
                    }
                }
                break;
            case AttackState.Atk3:
            case AttackState.Atk6:
                ComboCount = 0;
                break;
        }

    }

    void AirborneMovement()
    {
        GroundCheckDistance = RB.velocity.y < 0f ? OriginalGroundCheckDistance : 0.5f;
        Vector3 extraGravityForce = (Physics.gravity * GravityMultiplier) - Physics.gravity;
        RB.AddForce(extraGravityForce);
    }

    void ApplyTurnRotation()
    {
        float turnSpeed = 0f;
        if (Crouching)
        {
            turnSpeed = CrouchingTurnSpeed;
        }
        else
        {
            turnSpeed = Mathf.Lerp(StationaryTurnSpeed, MovingTurnSpeed, ForwardAmount);
        }
        transform.Rotate(0, TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void UpdateAnimator(Vector3 move)
    {
        MyAnimator.SetFloat("Forward", ForwardAmount, 0.5f, Time.fixedDeltaTime);
        MyAnimator.SetFloat("Turn", TurnAmount, 0.1f, Time.fixedDeltaTime);
        MyAnimator.SetBool("Crouch", Crouching);
        MyAnimator.SetBool("OnGround", Grounded);
        if (move.sqrMagnitude > 0.1f)
        {
            // When moving
            MyAnimator.SetFloat("RunSpeed", RunSpeed * 0.25f, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            // When Idle
            MyAnimator.SetFloat("RunSpeed", 1f, 0.2f, Time.fixedDeltaTime);
        }
        if (!Grounded)
        {
            MyAnimator.SetFloat("Jump", RB.velocity.y);
        }

        if (Grounded && move.magnitude > 0f)
        {
            MyAnimator.speed = AnimSpeedMultiplier;
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        Vector3 point = transform.position + Vector3.up * 0.5f;
        Vector3 dir = Vector3.down;
#if UNITY_EDITOR
        Debug.DrawLine(point, point + dir * GroundCheckDistance, Color.red);
#endif
        if (Physics.Raycast(point, dir, out hit, GroundCheckDistance))
        {
            Grounded = true;
            GroundNormal = hit.normal;
        }
        else
        {
            Grounded = false;
            GroundNormal = Vector3.up;
        }
    }

    void ScaleCapsule(bool crouch)
    {
        if (!Grounded)
        {
            Capsule.height = CapsuleHeight * 0.65f;
            Capsule.center = CapsuleCenter * 1.2f;
        }
        else if (Grounded && crouch)
        {
            if (Crouching)
            {
                return;
            }
            Capsule.height = Capsule.height * 0.5f;
            Capsule.center = Capsule.center * 0.5f;
            Crouching = true;
        }
        else
        {
            // Prevent standing in low headroom
            Ray crouchRay = new Ray(RB.position + Vector3.up * Capsule.height * 0.5f, Vector3.up);
            float rayLength = CapsuleHeight - Capsule.radius * 0.5f;
            if (Physics.SphereCast(crouchRay, Capsule.radius * 0.5f, rayLength, ~0, QueryTriggerInteraction.Ignore))
            {
                Crouching = true;
                return;
            }
            Capsule.height = CapsuleHeight;
            Capsule.center = CapsuleCenter;
            Crouching = false;
        }
    }

    public void Hurt(float amount)
    {
        HP -= amount;
    }

    public void Heal(float amount)
    {
        HP = Mathf.Clamp(HP + amount, 0, MaxHP);
    }

    public float GetNormalizedHealth()
    {
        return Mathf.Clamp01(HP / MaxHP);
    }
}
