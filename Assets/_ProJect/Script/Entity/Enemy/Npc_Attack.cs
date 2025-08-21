using System;
using UnityEngine;

public class Npc_Attack : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform head;

    [SerializeField] private float distanceRayAttackMelee = 2.25f;
    [SerializeField] private LayerMask shieldLayer;

    public event Action OnTryAnimationAttack;

    private Npc_Animation npc_Animation;
    private State_FollowEnemyTarget state_FollowTarget;

    private void Start()
    {
        state_FollowTarget = GetComponentInChildren<State_FollowEnemyTarget>();
        if(state_FollowTarget != null) state_FollowTarget.OnTryMeleeAttack += OnTryMeleeAttack;

        npc_Animation = GetComponentInChildren<Npc_Animation>();
        if(npc_Animation != null) npc_Animation.OnDoAttackMelee += OnAttackMelee;
    }

    private void OnTryMeleeAttack() => OnTryAnimationAttack?.Invoke();

    public void OnAttackMelee()
    {
        if (Physics.Raycast(head.position,transform.forward, out RaycastHit hit,distanceRayAttackMelee, shieldLayer))
        {
            if (hit.collider.TryGetComponent(out LifeSistem life)) life.UpdateHp(-stats.DamageMelee);

            if (hit.collider.TryGetComponent(out I_Team target)) target.SetPriorityTarget(transform);
        }
    }

    private void OnDisable()
    {
        if (state_FollowTarget != null) state_FollowTarget.OnTryMeleeAttack -= OnTryMeleeAttack;

        if (npc_Animation != null) npc_Animation.OnDoAttackMelee -= OnAttackMelee;
    }
}
