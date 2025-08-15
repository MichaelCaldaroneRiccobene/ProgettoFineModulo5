using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parametetTriggerAttack = "SecondAttack";
    [SerializeField] private string parameterTriggerOnHit = "OnHit";

    [SerializeField] private float smoothAnimation = 0.1f;

    public UnityEvent OnAttackMelee;

    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttack = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();
    }

    public void TryAttack()
    {
        if (isAttack) return;

        isAttack = true;
        animator.SetTrigger(parametetTriggerAttack);
    }

    public void OnAttackFinish() => isAttack = false;

    public void OnAttack() => OnAttackMelee?.Invoke();

    public void TriggerHit()
    {
        animator.SetTrigger(parameterTriggerOnHit);

        isAttack = false;
    }

    private void Update()
    {
        if (animator != null) animator.SetFloat(parameterFloatSpeed, agent.velocity.magnitude, smoothAnimation, Time.deltaTime);
    }
}
