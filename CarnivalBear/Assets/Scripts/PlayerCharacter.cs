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
    [SerializeField]
    float Damage;
    [SerializeField]
    float JumpDamage;
    [SerializeField]
    float ComboDamageMultiplier;

    Rigidbody RB;
    Animator MyAnimator;
    CapsuleCollider Capsule;
    PlayerAnimHashIDs AnimHash;

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
    bool ComboActive;
    AttackState CurrentAttackState;
    float ComboTimer;
    int ComboCount;

    //public Transform KneeRightIKHint;
    //public Transform KneeLeftIKHint;
    [SerializeField]
    GameObject RightChainsawTrigger;
    [SerializeField]
    GameObject LeftChainsawTrigger;

    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        AnimHash = GetComponent<PlayerAnimHashIDs>();
        MyAnimator.stabilizeFeet = true;
        MyAnimator.applyRootMotion = false;

        RB = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        CapsuleHeight = Capsule.height;
        CapsuleCenter = Capsule.center;
        OriginalGroundCheckDistance = GroundCheckDistance;
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

        HP = 1000f;

        RightChainsawTrigger.SetActive(false);
        LeftChainsawTrigger.SetActive(false);
    }

    // TODO fix IK
    //void Update()
    //{
    //    MyAnimator.SetIKHintPosition(AvatarIKHint.RightKnee, KneeRightIKHint.position);
    //    MyAnimator.SetIKHintPosition(AvatarIKHint.LeftKnee, KneeLeftIKHint.position);
    //    MyAnimator.SetIKHintPositionWeight(AvatarIKHint.RightKnee, 1f);
    //    MyAnimator.SetIKHintPositionWeight(AvatarIKHint.LeftKnee, 1f);
    //}

    public void Move(Vector3 move, bool crouch, bool jump)
    {
        CheckGrounded();
        int animState = MyAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash;
        if (animState == AnimHash.Atk3State || animState == AnimHash.Atk6State)
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
            //if (attack)
            //{
            //    friction *= 10f;
            //}
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

    public void Attack(bool attack)
    {
        // Find current state
        AnimatorStateInfo baseState = MyAnimator.GetCurrentAnimatorStateInfo(0);
        AnimatorStateInfo atkState = MyAnimator.GetCurrentAnimatorStateInfo(1);
        int animState = 0;
        float animNormalizedTime = 0f;
        float transitionTime = 0f;
        if (atkState.fullPathHash == AnimHash.AtkIdleState)
        {
            animState = baseState.fullPathHash;
            animNormalizedTime = baseState.normalizedTime - Mathf.Floor(baseState.normalizedTime);
            transitionTime = MyAnimator.GetAnimatorTransitionInfo(0).normalizedTime;
        }
        else
        {
            animState = atkState.fullPathHash;
            animNormalizedTime = atkState.normalizedTime - Mathf.Floor(atkState.normalizedTime);
            transitionTime = MyAnimator.GetAnimatorTransitionInfo(1).normalizedTime;
        }

        // Turn off colliders if not attacking and start timer if starting an attack
        if (!attack && !ComboActive)
        {
            RightChainsawTrigger.SetActive(false);
            LeftChainsawTrigger.SetActive(false);
            return;
        }
        else if (attack && !ComboActive)
        {
            ComboTimer = ComboTime;
            ComboCount = 0;
        }


        ComboActive = true;
        ComboTimer -= Time.fixedDeltaTime;
        if (ComboTimer < 0f)
        {
            ComboTimer = ComboTime;
            ComboCount = 0;
            ComboActive = false;
        }

        if (!attack) // If attack not pressed continue with combo timer
        {
            return;
        }
        else if (AnimHash.IsAnyAtkState(animState) && animNormalizedTime < 0.25f) // Button slaming breaks the combo
        {
            ComboActive = false;
            ComboCount = 0;
            return;
        }
        else // Good combo timing
        {
            ComboTimer = ComboTime;
            ComboCount++;
        }
        Debug.Log("ComboCount" + ComboCount + " animNormalizedTime:" + animNormalizedTime);

        // Do attack
        float q = Random.value;
        if (animState == AnimHash.CrouchState)
        {
            RightChainsawTrigger.SetActive(true);
            MyAnimator.SetTrigger(AnimHash.AtkCrouchTrigger);
        }
        else if (!Grounded /*animState == AnimHash.AirborneState*/ ) // No idea why checking Airborne state fails.  Hashes are fine but it just doesn't work.
        {
            RightChainsawTrigger.SetActive(true);
            LeftChainsawTrigger.SetActive(true);
            MyAnimator.SetTrigger(AnimHash.AtkJumpTrigger);
        }
        else if (animState == AnimHash.Atk1State || animState == AnimHash.Atk4State)
        {
            RightChainsawTrigger.SetActive(false);
            LeftChainsawTrigger.SetActive(true);
            if (ComboCount == 3)
            {
                MyAnimator.SetTrigger(AnimHash.Atk3Trigger);
            }
            else
            {
                if (q < 0.5f)
                {
                    MyAnimator.SetTrigger(AnimHash.Atk2Trigger);
                }
                else
                {
                    MyAnimator.SetTrigger(AnimHash.Atk5Trigger);
                }
            }
        }
        else if (animState == AnimHash.Atk2State || animState == AnimHash.Atk5State)
        {
            RightChainsawTrigger.SetActive(true);
            LeftChainsawTrigger.SetActive(false);
            if (ComboCount == 3)
            {
                MyAnimator.SetTrigger(AnimHash.Atk6Trigger);
            }
            else
            {
                if (q < 0.5f)
                {
                    MyAnimator.SetTrigger(AnimHash.Atk1Trigger);
                }
                else
                {
                    MyAnimator.SetTrigger(AnimHash.Atk4Trigger);
                }
            }
        }
        else if (animState == AnimHash.Atk3State || animState == AnimHash.Atk6State)
        {
            ComboCount = 0;
            ComboActive = false;
        }
        else
        {
            if (q < 0.25f)
            {
                MyAnimator.SetTrigger(AnimHash.Atk1Trigger);
                RightChainsawTrigger.SetActive(true);
            }
            else if (q < 0.5f)
            {
                MyAnimator.SetTrigger(AnimHash.Atk2Trigger);
                LeftChainsawTrigger.SetActive(true);
            }
            else if (q < 0.75f)
            {
                MyAnimator.SetTrigger(AnimHash.Atk4Trigger);
                RightChainsawTrigger.SetActive(true);
            }
            else
            {
                MyAnimator.SetTrigger(AnimHash.Atk5Trigger);
                LeftChainsawTrigger.SetActive(true);
            }
        }

        //switch (CurrentAttackState)
        //{
        //    case AttackState.Idle:
        //        if (q < 0.25f)
        //        {
        //            MyAnimator.SetTrigger(AnimHash.Atk1Trigger);
        //            CurrentAttackState = AttackState.Atk1;
        //        }
        //        else if (q < 0.5f)
        //        {
        //            MyAnimator.SetTrigger(AnimHash.Atk2Trigger);
        //            CurrentAttackState = AttackState.Atk2;
        //        }
        //        else if (q < 0.75f)
        //        {
        //            MyAnimator.SetTrigger(AnimHash.Atk4Trigger);
        //            CurrentAttackState = AttackState.Atk4;
        //        }
        //        else
        //        {
        //            MyAnimator.SetTrigger(AnimHash.Atk5Trigger);
        //            CurrentAttackState = AttackState.Atk5;
        //        }
        //        break;
        //    case AttackState.Atk1:
        //    case AttackState.Atk4:
        //        if (ComboCount == 3)
        //        {
        //            MyAnimator.SetTrigger(AnimHash.Atk3Trigger);
        //            CurrentAttackState = AttackState.Atk3;
        //        }
        //        else
        //        {
        //            if (q < 0.5f)
        //            {
        //                MyAnimator.SetTrigger(AnimHash.Atk2Trigger);
        //                CurrentAttackState = AttackState.Atk2;
        //            }
        //            else
        //            {
        //                MyAnimator.SetTrigger(AnimHash.Atk5Trigger);
        //                CurrentAttackState = AttackState.Atk5;
        //            }
        //        }
        //        break;
        //    case AttackState.Atk2:
        //    case AttackState.Atk5:
        //        if (ComboCount == 3)
        //        {
        //            MyAnimator.SetTrigger(AnimHash.Atk6Trigger);
        //            CurrentAttackState = AttackState.Atk6;
        //        }
        //        else
        //        {
        //            if (q < 0.5f)
        //            {
        //                MyAnimator.SetTrigger(AnimHash.Atk1Trigger);
        //                CurrentAttackState = AttackState.Atk1;
        //            }
        //            else
        //            {
        //                MyAnimator.SetTrigger(AnimHash.Atk4Trigger);
        //                CurrentAttackState = AttackState.Atk4;
        //            }
        //        }
        //        break;
        //    case AttackState.Atk3:
        //    case AttackState.Atk6:
        //        ComboCount = 0;
        //        break;
        //}
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
        MyAnimator.SetFloat(AnimHash.ForwardFloat, ForwardAmount, 0.5f, Time.fixedDeltaTime);
        MyAnimator.SetFloat(AnimHash.TurnFloat, TurnAmount, 0.1f, Time.fixedDeltaTime);
        MyAnimator.SetBool(AnimHash.CrouchBool, Crouching);
        MyAnimator.SetBool(AnimHash.OnGroundBool, Grounded);
        if (move.sqrMagnitude > 0.1f)
        {
            // When moving
            MyAnimator.SetFloat(AnimHash.RunSpeedFloat, RunSpeed * 0.25f, 0.2f, Time.fixedDeltaTime);
        }
        else
        {
            // When Idle
            MyAnimator.SetFloat(AnimHash.RunSpeedFloat, 1f, 0.2f, Time.fixedDeltaTime);
        }
        if (!Grounded)
        {
            MyAnimator.SetFloat(AnimHash.JumpFloat, RB.velocity.y);
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

    public float GetDamage()
    {
        if (MyAnimator.GetCurrentAnimatorStateInfo(0).fullPathHash == AnimHash.AtkAirState)
        {
            return JumpDamage;
        }
        else
        {
            return Damage * ComboCount * ComboDamageMultiplier;
        }
    }
}
