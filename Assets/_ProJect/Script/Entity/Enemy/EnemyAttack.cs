using System;
using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceRayAttackMelee = 2.25f;
    [SerializeField] private LayerMask shieldLayer;

    public event Action OnTryAnimationAttack;

    private EnemyAnimation enemyAnimation;
    private State_FollowEnemyTarget state_FollowTarget;

    private void Start()
    {
        state_FollowTarget = GetComponentInChildren<State_FollowEnemyTarget>();
        if(state_FollowTarget != null) state_FollowTarget.OnTryMeleeAttack += OnTryMeleeAttack;

        enemyAnimation = GetComponentInChildren<EnemyAnimation>();
        if(enemyAnimation != null) enemyAnimation.OnDoAttackMelee += OnAttackMelee;
    }

    private void OnTryMeleeAttack() => OnTryAnimationAttack?.Invoke();

    public void OnAttackMelee()
    {
        Debug.Log("Attac");
        if (Physics.Raycast(head.position,transform.forward, out RaycastHit hit,distanceRayAttackMelee, shieldLayer))
        {
            if (hit.collider.TryGetComponent(out LifeSistem life)) life.UpdateHp(-stats.DamageMelee);

            if (hit.collider.TryGetComponent(out I_Team target)) target.SetTargetPriority(transform);
        }
    }

    private void OnDisable()
    {
        if (state_FollowTarget != null) state_FollowTarget.OnTryMeleeAttack += OnTryMeleeAttack;

        if (enemyAnimation != null) enemyAnimation.OnDoAttackMelee -= OnAttackMelee;
    }
}
