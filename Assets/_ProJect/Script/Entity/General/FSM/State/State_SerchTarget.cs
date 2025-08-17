using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_SerchTarget : AbstractState
{
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private NavMeshAgent agent;
    private Vector3 pointToGo;

    public override void StateEnter()
    {
        Debug.Log("Entrato in State SerchTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        agent.ResetPath();
        StartCoroutine(GoOnSerchTargetRoutin());
    }

    public override void StateExit()
    {
        Debug.Log("Uscito dallo State SerchTarget");
        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnSerchTargetRoutin()
    {
        if(controller.LastTarget == null) yield break;

        while (controller.LastTarget)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
            bool isOnGoRandomPoint = false;

            RandomPoint(agent, controller.LastTarget.position, radiusRandomPosition, out pointToGo);
            agent.SetDestination(pointToGo);

            while (agent.pathPending) yield return null;

            isOnGoRandomPoint = true;

            while (isOnGoRandomPoint)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoRandomPoint = false;

                yield return waitForSeconds;
            }
            controller.LastTarget = null;
        }
    }

    private void RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range, out Vector3 result)
    {
        int numberOfTentativ = 100;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
            randomPosition.y = agent.transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return;
            }
        }
        result = Vector3.zero;
    }
}
