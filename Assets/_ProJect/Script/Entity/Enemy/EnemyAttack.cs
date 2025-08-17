using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceRayAttackMelee = 2.25f;

    public event Action OnTryAnimationAttack;

    private EnemyAnimation enemyAnimation;
    private State_FollowTarget state_FollowTarget;

    private void Start()
    {
        state_FollowTarget = GetComponentInChildren<State_FollowTarget>();
        state_FollowTarget.OnTryMeleeAttack += OnTryMeleeAttack;

        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        enemyAnimation.OnDoAttackMelee += OnAttackMelee;
    }

    private void OnTryMeleeAttack() => OnTryAnimationAttack?.Invoke();

    public void OnAttackMelee()
    {
        if(Physics.Raycast(head.position,transform.forward, out RaycastHit hit,distanceRayAttackMelee))
        {
            if (hit.collider.TryGetComponent(out LifeSistem life)) life.UpdateHp(-stats.DamageMelee);
        }
    }

    private void OnDisable()
    {
        state_FollowTarget.OnTryMeleeAttack += OnTryMeleeAttack;

        enemyAnimation.OnDoAttackMelee -= OnAttackMelee;
    }
}
