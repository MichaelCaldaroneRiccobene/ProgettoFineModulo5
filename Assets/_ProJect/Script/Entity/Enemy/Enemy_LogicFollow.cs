using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_LogicFollow : MonoBehaviour
{
    private NavMeshPath pathToFollw;

    private void Start() => pathToFollw = new NavMeshPath();

    public void GoOnTarget(NavMeshAgent agent, Transform target)
    {
        agent.ResetPath();
        agent.SetDestination(agent.transform.position);
        StartCoroutine(GoOnTargetRoutin(agent, target));
    }

    private IEnumerator GoOnTargetRoutin(NavMeshAgent agent,Transform target)
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        agent.ResetPath();

        while (target != null)
        {
            if (agent.CalculatePath(target.position, pathToFollw)) agent.destination = target.position;

            yield return waitForSeconds;
        }
    }
}
