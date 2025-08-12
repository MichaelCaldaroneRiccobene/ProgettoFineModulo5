using UnityEngine;

public class EnemyAttack : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceRayAttackMelee = 2.25f;

    private EnemyBrain brain;

    private void Start()
    {
        brain = GetComponent<EnemyBrain>();
    }

    public void OnAttackMelee()
    {
        if(Physics.Raycast(head.position,transform.forward, out RaycastHit hit,distanceRayAttackMelee))
        {
            if(hit.collider.TryGetComponent(out I_Team team))
            {
                Debug.DrawRay(head.position,hit.point,Color.green,1);
                if (brain.GetTeamNumber() != team.GetTeamNumber() )
                {
                    if (hit.collider.TryGetComponent(out LifeSistem life))
                    {
                        life.UpdateHp(-stats.DamageMelee);
                    }
                }
            }
        }
    }
}
