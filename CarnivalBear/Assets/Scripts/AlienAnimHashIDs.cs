using UnityEngine;
using System.Collections;

public class AlienAnimHashIDs : MonoBehaviour
{
    public int ForwardFloat;
    public int TurnFloat;
    public int ShootTrigger;
    public int DieTrigger;

    void Awake()
    {
        ForwardFloat = Animator.StringToHash("Forward");
        TurnFloat = Animator.StringToHash("Turn");
        ShootTrigger = Animator.StringToHash("Shoot");
        DieTrigger = Animator.StringToHash("Die");
    }
}