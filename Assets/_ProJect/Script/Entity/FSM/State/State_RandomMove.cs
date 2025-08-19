using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_RandomMove : AbstractState
{
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private NavMeshAgent agent;
    private Vector3 pointToGo;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State RandomMove");

        if(agent == null) agent = GetComponentInParent<NavMeshAgent>();

        agent.ResetPath();
        StartCoroutine(GoOnRandomPointRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State RandomMove");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnRandomPointRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        while (true)
        {
            bool isOnGoRandomPoint = false;

            Utility.RandomPoint(agent, agent.transform.position, radiusRandomPosition, out pointToGo);
            agent.SetDestination(pointToGo);

            while (agent.pathPending) yield return null;

            isOnGoRandomPoint = true;

            while (isOnGoRandomPoint)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoRandomPoint = false;

                yield return waitForSeconds;
            }
        }
    }
}
