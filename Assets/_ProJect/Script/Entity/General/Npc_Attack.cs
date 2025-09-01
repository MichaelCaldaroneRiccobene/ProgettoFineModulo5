using System;
using System.Collections;
using UnityEngine;

public class Npc_Attack : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform head;

    [SerializeField] private float distanceRayAttackMelee = 1.5f;

    public void OnAttackMelee()
    {
        if (Physics.Raycast(head.position, transform.forward, out RaycastHit hit, distanceRayAttackMelee))
        {
            Debug.DrawLine(head.position, hit.point, Color.blue,2f);
            if (hit.collider.TryGetComponent(out I_Damageble damageble)) damageble.Damage(-stats.DamageMelee);

            if (hit.collider.TryGetComponent(out I_Team target)) target.SetPriorityTarget(transform);
        }
    }
}
