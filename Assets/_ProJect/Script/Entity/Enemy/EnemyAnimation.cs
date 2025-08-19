using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyAnimation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parametetTriggerAttack = "SecondAttack";
    [SerializeField] private string parameterTriggerOnHit = "OnHit";
    [SerializeField] private string parameterTriggerOnTurn180 = "OnTurn180";

    [SerializeField] private float smoothAnimation = 0.1f;

    public event Action OnDoAttackMelee;

   [SerializeField] private State_StayInPlaceAndLoockAround state_StayInPlaceAndLoockAround;
    private EnemyAttack enemyAttack;
    private NavMeshAgent agent;
    private Animator animator;

    private bool isAttack = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponentInParent<NavMeshAgent>();

        enemyAttack = GetComponentInParent<EnemyAttack>();
        enemyAttack.OnTryAnimationAttack += TryAttack;

        if (state_StayInPlaceAndLoockAround != null) state_StayInPlaceAndLoockAround.OnTurn180 += TriggerTurn180;
    }

    private void Update()
    {
        if (animator != null) animator.SetFloat(parameterFloatSpeed, agent.velocity.magnitude, smoothAnimation, Time.deltaTime);
    }

    public void TryAttack()
    {
        if (isAttack) return;

        isAttack = true;
        animator.SetTrigger(parametetTriggerAttack);
    }

    public void OnAttackFinish() => isAttack = false;

    public void OnAttack() => OnDoAttackMelee?.Invoke();

    public void TriggerHit()
    {
        animator.SetTrigger(parameterTriggerOnHit);

        isAttack = false;
    }

    public void TriggerTurn180() => animator.SetTrigger(parameterTriggerOnTurn180);

    private void OnDisable()
    {
        enemyAttack.OnTryAnimationAttack -= TryAttack;

        if (state_StayInPlaceAndLoockAround != null) state_StayInPlaceAndLoockAround.OnTurn180 -= TriggerTurn180;
    }
}
