using UnityEngine;
using System.Collections;

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
    float MoveSpeedMultiplier = 1f;
    [SerializeField]
    float AnimSpeedMultiplier = 1f;
    [SerializeField]
    float JumpPower = 12f;
    [SerializeField]
    float Acceleration = 1f;
    [SerializeField]
    float MaxSpeed = 5f;

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
    Vector3 Velocity;

    void Start()
    {
        MyAnimator = GetComponent<Animator>();
        RB = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        CapsuleHeight = Capsule.height;
        CapsuleCenter = Capsule.center;
        OriginalGroundCheckDistance = GroundCheckDistance;
        RB.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void Move(Vector3 move, bool crouch, bool jump, bool attack)
    {
        Debug.Log(move);

        //Velocity = RB.velocity;
        //Velocity += move * Acceleration * Time.fixedDeltaTime;
        //float originalY = Velocity.y;
        //Velocity.y = 0;
        //Velocity = Vector3.ClampMagnitude(Velocity, MaxSpeed);
        //Velocity.y = originalY;

        CheckGrounded();
        
        move = transform.InverseTransformDirection(move);
        move = Vector3.ProjectOnPlane(move, GroundNormal);
        TurnAmount = Mathf.Atan2(move.x, move.z);


        // Root Motion Method
        ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        if (Grounded)
        {
            Jump(crouch, jump);
        }
        else
        {
            AirborneMovement();
        }

        ScaleCapsuleForCrouch(crouch);

        UpdateAnimator(move);
    }

    void FixedUpdate()
    {
        //RB.MovePosition(Velocity);
    }

    void Jump(bool crouch, bool jump)
    {
        if (jump && !crouch && MyAnimator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            RB.velocity = new Vector3(RB.velocity.x, JumpPower, RB.velocity.z);
            Grounded = false;
            MyAnimator.applyRootMotion = false;
            GroundCheckDistance = 0.1f;
        }
    }

    void AirborneMovement()
    {
        GroundCheckDistance = RB.velocity.y < 0f ? OriginalGroundCheckDistance : 0.1f;
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
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
        //Debug.Log("Forward:" + ForwardAmount + " TurnAmount:" + TurnAmount);
        //Debug.Log(RB.velocity.magnitude);
        MyAnimator.SetFloat("Forward", ForwardAmount, 0.1f, Time.deltaTime);
        MyAnimator.SetFloat("Turn", TurnAmount, 0.1f, Time.deltaTime);
        MyAnimator.SetBool("Crouch", Crouching);
        MyAnimator.SetBool("OnGround", Grounded);
        if (!Grounded)
        {
            MyAnimator.SetFloat("Jump", RB.velocity.y);
        }

        if (Grounded && move.magnitude > 0f)
        {
            MyAnimator.speed = AnimSpeedMultiplier;
        }
    }

    public void OnAnimatorMove()
    {
        if (Grounded && Time.deltaTime > 0f)
        {
            Vector3 v = (MyAnimator.deltaPosition * MoveSpeedMultiplier) / Time.deltaTime;
            v.y = RB.velocity.y;
            RB.velocity = v;
        }
    }

    void CheckGrounded()
    {
        RaycastHit hit;
        Vector3 point = transform.position + Vector3.up * 0.1f;
        Vector3 dir = Vector3.down;
#if UNITY_EDITOR
        Debug.DrawRay(point, dir, Color.red);
#endif
        if (Physics.Raycast(point, dir, out hit, GroundCheckDistance))
        {
            Grounded = true;
            GroundNormal = hit.normal;
            MyAnimator.applyRootMotion = true;
        }
        else
        {
            Grounded = false;
            GroundNormal = Vector3.up;
            MyAnimator.applyRootMotion = false;
        }
    }

    void ScaleCapsuleForCrouch(bool crouch)
    {
        if (Grounded && crouch)
        {
            if (Crouching)
            {
                return;
            }
            Capsule.height = Capsule.height / 2f;
            Capsule.center = Capsule.center / 2f;
            Crouching = true;
        }
        else
        {
            // Prevent standing in low headroom
            Ray crouchRay = new Ray(RB.position + Vector3.up * Capsule.radius * 0.5f, Vector3.up);
            float rayLength = CapsuleHeight - Capsule.radius * 0.5f;

            //TODO - need to verify this actually works

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
}
