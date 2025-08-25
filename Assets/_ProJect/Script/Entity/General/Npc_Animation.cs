using System;
using UnityEngine;
using UnityEngine.AI;

public class Npc_Animation : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] protected string parameterFloatSpeed = "Speed";

    [SerializeField] protected string parametetTriggerAttack = "Attack";
    [SerializeField] protected string parameterTriggerOnHit = "OnHit";

    [SerializeField] protected string parameterTriggerOnDead = "OnDead";

    [SerializeField] protected float smoothAnimation = 0.04f;

    public event Action OnDoAttackMelee;

    protected NavMeshAgent agent;
    protected Animator animator;

    protected State_Dead state_Dead;
    protected Npc_Attack npc_Attack;

    protected bool isAttack = false;

    protected Vector3 localVelocity;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();

        npc_Attack = GetComponentInParent<Npc_Attack>();
        if(npc_Attack != null) npc_Attack.OnTryAnimationAttack += TryAttack;

        if(npc_Attack != null) state_Dead = npc_Attack.GetComponentInChildren<State_Dead>();
        if(state_Dead != null) state_Dead.OnTriggerDead += OnTriggerDead;
    }

    public virtual void Update()
    {
        AnimationMoving();
    }

    private void AnimationMoving()
    {
        if (animator == null) return;

        localVelocity = transform.InverseTransformDirection(agent.velocity);

        float vertical = localVelocity.z * agent.speed;
        vertical = Mathf.Clamp(vertical, -1f, 1f);

        if (animator != null) animator.SetFloat(parameterFloatSpeed, vertical, smoothAnimation, Time.deltaTime);
    }

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

    private void OnTriggerDead() => animator.SetTrigger(parameterTriggerOnDead);

    public virtual void OnDisable()
    {
        if (npc_Attack != null) npc_Attack.OnTryAnimationAttack -= TryAttack;

        if (state_Dead != null) state_Dead.OnTriggerDead -= OnTriggerDead;
    }
}
