using UnityEngine;
using UnityEngine.AI;

public class Entity_Movement : MonoBehaviour
{
    public void Movement(Vector3 direction,NavMeshAgent agent) => agent.velocity = direction.normalized * agent.speed;
}
