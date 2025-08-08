using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using UnityEngine;
using UnityEngine.AI;

public class Enemy_LogicRandomMove : MonoBehaviour
{
    [SerializeField] private float radiusRandomPosition = 15;
    private Vector3 pointToGo;

    public void GoOnRandomPoint(NavMeshAgent agent)
    {
        agent.ResetPath();
        agent.SetDestination(agent.transform.position);
        StartCoroutine(GoOnRandomPointRoutin(agent));
    }

    private IEnumerator GoOnRandomPointRoutin(NavMeshAgent agent)
    {
        while(true)
        {
            bool isOnGoRandomPoint = false;

            RandomPoint(agent,agent.transform.position, radiusRandomPosition, out pointToGo);
            while (agent.pathPending) yield return null;

            isOnGoRandomPoint = true;

            while (isOnGoRandomPoint)
            {
                agent.SetDestination(pointToGo);
                if (agent.remainingDistance < 1) isOnGoRandomPoint = false;

                yield return null;
            }
        }
    }

    private void RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range, out Vector3 result)
    {
        Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
        randomPosition.y = agent.transform.position.y;

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas)) result = hit.position;
        else result = Vector3.zero;
    }
}
