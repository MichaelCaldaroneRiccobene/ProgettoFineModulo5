using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_ControlSingelton : MonoBehaviour
{
    public static AI_ControlSingelton Instance;

    public Transform target;

    private NavMeshPath pathToFollow;

    private bool oneTime = false;

    private void Awake()
    {
        Instance = this;
        pathToFollow = new NavMeshPath();
    }

    public void GoOnRandomPosition(NavMeshAgent agent)
    {
        //StartCoroutine(GoOnRandomPositionRoutine(agent));

        if(!oneTime)
        {
            oneTime = true; 
            StopAllCoroutines();
            Vector3 point;
            if (RandomPoint(transform.position, 15, out point)) agent.SetDestination(point);
        }
    }

    public void GoOnTarget(NavMeshAgent agent)
    {
        StartCoroutine(GoOnTargetRoutine(agent));
        oneTime = false;
    }

    private IEnumerator GoOnTargetRoutine(NavMeshAgent agent)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        agent.ResetPath();

        while(target != null)
        {
            if(agent.CalculatePath(target.position,pathToFollow))
            {
                agent.destination = target.position;
            }

            yield return waitForSeconds;
        }
    }

    private IEnumerator GoOnRandomPositionRoutine(NavMeshAgent agent)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(2);
        agent.ResetPath();

        while (true)
        {
            if(agent.remainingDistance <= agent.stoppingDistance)
            {
                Vector3 point;
                if (RandomPoint(transform.position, 15, out point)) agent.SetDestination(point);
            }

            yield return waitForSeconds;
        }
    }

    private bool RandomPoint(Vector3 startPosition, float range, out Vector3 result)
    {
        Vector3 randomPosition = Random.insideUnitSphere * range;

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit,1, NavMesh.AllAreas))
        {
            result = hit.position;
            return true;
        }

        result = Vector3.zero;
        return false;
    }
}
