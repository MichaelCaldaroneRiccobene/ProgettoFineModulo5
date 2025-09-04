using UnityEngine;
using UnityEngine.Events;

public class Allied_Animation : Npc_Animation
{
    [Header("Setting Allied_Animation")]
    [SerializeField] private string parameterFloatDirection = "Direction";
    [SerializeField] private string parameterTriggerOnSitUp = "OnSitUp";

    [SerializeField] private string parameterTriggerOnAttack = "OnAttack";

    public UnityEvent OnDoDamageAttack;
    public UnityEvent OnCanWalk;

    public override void AnimationMoving()
    {
        base.AnimationMoving();

        horizontal = Mathf.Clamp(horizontal, -1f, 1f);
        float speedHorizontal = (agent.velocity.magnitude / agent.speed) * horizontal;

        if (animator != null) animator.SetFloat(parameterFloatDirection, speedHorizontal, smoothAnimation, Time.deltaTime);
    }

    public void OnAttack()
    {
        if (isAttacking) return;
        DoAttack(parameterTriggerOnAttack);
    }

    public void OnDoDamageAttackAnimation() => OnDoDamageAttack?.Invoke();

    public void OnTriggerSitUp() => animator.SetTrigger(parameterTriggerOnSitUp);
    public void OnSitUp() => OnCanWalk?.Invoke();

}
