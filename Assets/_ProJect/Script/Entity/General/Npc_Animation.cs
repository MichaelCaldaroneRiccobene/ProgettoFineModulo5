using UnityEngine;
using UnityEngine.AI;
public class Npc_Animation : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] protected string parameterFloatSpeed = "Speed";

    [SerializeField] protected string parameterTriggerOnHit = "OnHit";
    [SerializeField] private string parameterTriggerOnDead = "OnDead";

    [SerializeField] protected float smoothAnimation = 0.1f;

    protected NavMeshAgent agent;
    protected Animator animator;

    protected LifeSistem lifeSistem;

    protected Vector3 localVelocity;
    protected float vertical;
    protected float horizontal;

    protected bool isAttacking;

    public virtual void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();

        SetUpAction();
    }

    public virtual void Update() => AnimationMoving();

    public virtual void SetUpAction()
    {
        lifeSistem = GetComponentInParent<LifeSistem>();

        if(lifeSistem != null) lifeSistem.OnHit += OnHit;
        if (lifeSistem != null) lifeSistem.OnDead += OnDead;
    }

    public virtual void DoAttack(string parameter)
    {
        isAttacking = true;
        animator.SetTrigger(parameter);
    }

    public virtual void OnFinishAttack() => isAttacking = false;

    public virtual void OnHit()
    {
        isAttacking = false;
        animator.SetTrigger(parameterTriggerOnHit);
    }

    public virtual void OnFinishHit() => isAttacking = false;

    public virtual void OnDead() => animator.SetTrigger(parameterTriggerOnDead);

    public virtual void AnimationMoving()
    {
        if (animator == null) return;

        Vector3 localVelocity = transform.InverseTransformDirection(agent.velocity);

        vertical = localVelocity.z;
        horizontal = localVelocity.x;

        vertical = Mathf.Clamp(vertical, -1f, 1f);

        float speedVertical = (agent.velocity.magnitude / agent.speed) * vertical;
        if (animator != null) animator.SetFloat(parameterFloatSpeed, speedVertical, smoothAnimation, Time.deltaTime);
    }

    public virtual void OnDisable()
    {
        if (lifeSistem != null) lifeSistem.OnHit -= OnHit;
        if (lifeSistem != null) lifeSistem.OnDead -= OnDead;
    }
}
