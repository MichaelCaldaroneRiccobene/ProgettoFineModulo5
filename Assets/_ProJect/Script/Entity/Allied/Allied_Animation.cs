using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Allied_Animation : MonoBehaviour
{
    [SerializeField] private string parameterFloatSpeed = "Speed";
    [SerializeField] private string parametetTriggerAttack = "Attack";
    [SerializeField] private string parameterTriggerOnHit = "OnHit";

    [SerializeField] private string parameterTriggerSitUp = "SitUp";

    [SerializeField] private float smoothAnimation = 0.04f;

    public event Action OnDoAttackMelee;
    public UnityEvent OnDoBadAttackMelee;


    public UnityEvent OnDoSitUp;

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

    public void OnAttack()
    {
        OnDoAttackMelee?.Invoke();
        OnDoBadAttackMelee?.Invoke();
    }

    public void TriggerHit()
    {
        animator.SetTrigger(parameterTriggerOnHit);

        isAttack = false;
    }


    public void TriggerSitUp() => animator.SetTrigger(parameterTriggerSitUp);
    public void OnSitUp()
    {
        Debug.Log("Allied SitUp");
        OnDoSitUp?.Invoke();
    }

    private void OnDisable()
    {
        enemyAttack.OnTryAnimationAttack -= TryAttack;
    }
}
