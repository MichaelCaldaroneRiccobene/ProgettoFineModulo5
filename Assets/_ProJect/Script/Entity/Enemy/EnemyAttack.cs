using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceRayAttackMelee = 2.25f;

    public void OnAttackMelee()
    {
        Debug.Log("!");
        if(Physics.Raycast(head.position,transform.forward, out RaycastHit hit,distanceRayAttackMelee))
        {
            Debug.DrawRay(head.position, hit.point,Color.green,1);
            if (hit.collider.TryGetComponent(out LifeSistem life))
            {
                life.UpdateHp(-stats.DamageMelee);
            }
        }
    }
}
