using UnityEngine;
using System.Collections;

public class PlayerAnimHashIDs : MonoBehaviour
{
    public int Atk1State;
    public int Atk2State;
    public int Atk3State;
    public int Atk4State;
    public int Atk5State;
    public int Atk6State;
    public int AtkCrouchState;
    public int AtkAirState;
    public int AtkIdleState;
    public int GroundedState;
    public int AirborneState;
    public int CrouchState;
    public int StunnedState;
    public int StunBreak;
    public int DieState;

    public int ForwardFloat;
    public int TurnFloat;
    public int CrouchBool;
    public int OnGroundBool;
    public int JumpFloat;
    public int RunSpeedFloat;
    public int StunFloat;
    public int Atk1Trigger;
    public int Atk2Trigger;
    public int Atk3Trigger;
    public int Atk4Trigger;
    public int Atk5Trigger;
    public int Atk6Trigger;
    public int AtkCrouchTrigger;
    public int AtkJumpTrigger;
    public int DieTrigger;

    void Awake()
    {
        //States
        Atk1State = Animator.StringToHash("AttackLayer.Atk1");
        Atk2State = Animator.StringToHash("AttackLayer.Atk2");
        Atk3State = Animator.StringToHash("Base Layer.Atk3");
        Atk4State = Animator.StringToHash("AttackLayer.Atk4");
        Atk5State = Animator.StringToHash("AttackLayer.Atk5");
        Atk6State = Animator.StringToHash("Base Layer.Atk6");
        AtkAirState = Animator.StringToHash("Base Layer.AtkAir");
        AtkCrouchState = Animator.StringToHash("Base Layer.AtCrouch");
        AtkIdleState = Animator.StringToHash("AttackLayer.AtkIdle");
        CrouchState = Animator.StringToHash("Base Layer.Crouch");
        AirborneState = Animator.StringToHash("Base Layer.Airborne");
        GroundedState = Animator.StringToHash("Base Layer.Grounded");
        StunnedState = Animator.StringToHash("Base Layer.Stunned");
        StunBreak = Animator.StringToHash("Base Layer.StunBreak");
        DieState = Animator.StringToHash("Base Layer.Die");

        // Parameters
        ForwardFloat = Animator.StringToHash("Forward");
        TurnFloat = Animator.StringToHash("Turn");
        CrouchBool = Animator.StringToHash("Crouch");
        OnGroundBool = Animator.StringToHash("OnGround");
        JumpFloat = Animator.StringToHash("Jump");
        RunSpeedFloat = Animator.StringToHash("RunSpeed");
        StunFloat = Animator.StringToHash("Stunned");
        Atk1Trigger = Animator.StringToHash("Atk1Trigger");
        Atk2Trigger = Animator.StringToHash("Atk2Trigger");
        Atk3Trigger = Animator.StringToHash("Atk3Trigger");
        Atk4Trigger = Animator.StringToHash("Atk4Trigger");
        Atk5Trigger = Animator.StringToHash("Atk5Trigger");
        Atk6Trigger = Animator.StringToHash("Atk6Trigger");
        AtkCrouchTrigger = Animator.StringToHash("AtkCrouchTrigger");
        AtkJumpTrigger = Animator.StringToHash("AtkJumpTrigger");
        DieTrigger = Animator.StringToHash("DieTrigger");
    }

    public bool IsAnyAtkState(int stateHash)
    {
        if(stateHash==Atk1State)
        {
            return true;
        }
        else if (stateHash == Atk2State)
        {
            return true;
        }
        else if (stateHash == Atk3State)
        {
            return true;
        }
        else if (stateHash == Atk4State)
        {
            return true;
        }
        else if (stateHash == Atk5State)
        {
            return true;
        }
        else if (stateHash == Atk6State)
        {
            return true;
        }
        else if (stateHash == AtkCrouchState)
        {
            return true;
        }
        else if (stateHash == AtkJumpTrigger)
        {
            return true;
        }
        return false;
    }
}
