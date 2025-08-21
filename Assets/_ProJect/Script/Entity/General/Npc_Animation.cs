using System;
using UnityEngine;
using UnityEngine.AI;

public class Npc_Animation : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] protected string parameterFloatSpeed = "Speed";

    [SerializeField] protected string parametetTriggerAttack = "Attack";
    [SerializeField] protected string parameterTriggerOnHit = "OnHit";

    [SerializeField] protected float smoothAnimation = 0.04f;

    public event Action OnDoAttackMelee;

    protected NavMeshAgent agent;
    protected Animator animator;

    protected Npc_Attack npc_Attack;

    protected bool isAttack = false;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();

        npc_Attack = GetComponentInParent<Npc_Attack>();
        if(npc_Attack != null) npc_Attack.OnTryAnimationAttack += TryAttack;
    }

    public virtual void Update() {if (animator != null) animator.SetFloat(parameterFloatSpeed, agent.velocity.magnitude, smoothAnimation, Time.deltaTime);}

    public virtual void TryAttack()
    {
        if (isAttack) return;

        isAttack = true;
        animator.SetTrigger(parametetTriggerAttack);
    }

    public virtual void OnAttack() => OnDoAttackMelee?.Invoke();
    public virtual void OnAttackFinish() => isAttack = false;


    public virtual void TriggerHit()
    {
        animator.SetTrigger(parameterTriggerOnHit);

        isAttack = false;
    }

    public virtual void OnDisable()
    {
        if (npc_Attack != null) npc_Attack.OnTryAnimationAttack -= TryAttack;
    }
}
