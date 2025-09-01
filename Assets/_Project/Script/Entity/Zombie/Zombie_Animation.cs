using UnityEngine;
using UnityEngine.Events;

public class Zombie_Animation : Npc_Animation
{
    [SerializeField] private string parameterTriggerTurn = "OnTurn";
    [SerializeField] private string parameterTriggerOnAttack = "OnAttack";

    public UnityEvent OnDoDamageAttack;

    public void OnTurn() => animator.SetTrigger(parameterTriggerTurn);

    public void OnAttack()
    {
        if (isAttacking) return;

        DoAttack(parameterTriggerOnAttack);
    }

    public void OnDoDamageAttackAnimation() => OnDoDamageAttack?.Invoke();

    public override void OnHit()
    {
        base.OnHit();
        isAttacking = false;
    }
}
