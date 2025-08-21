using UnityEngine;

public class Zombie_Animation : Npc_Animation
{
    [SerializeField] private string parameterTriggerOnTurn180 = "OnTurn180";

    public void TriggerTurn180() => animator.SetTrigger(parameterTriggerOnTurn180);
}
