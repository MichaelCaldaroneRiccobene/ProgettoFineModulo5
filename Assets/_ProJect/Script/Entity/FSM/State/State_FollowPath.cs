using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowPath : AbstractState
{
    [SerializeField] private Transform[] pointsForPatrol;

    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private NavMeshPath pathToFollw;
    private NavMeshAgent agent;
    private Vector3 pointToGo;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowPath");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();
        if(pathToFollw == null) pathToFollw = new NavMeshPath();

        agent.ResetPath();
        StartCoroutine(GoOnPatrolRoutine());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowPath");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnPatrolRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        if (pointsForPatrol.Length <= 0) yield break;

        int destinationForPatrolIndex = 0;

        while (true)
        {
            bool isOnGoOnPatrol = false;

            if (agent.CalculatePath(pointsForPatrol[destinationForPatrolIndex].position, pathToFollw)) pointToGo = pointsForPatrol[destinationForPatrolIndex].position;
            while (agent.pathPending) yield return null;

            isOnGoOnPatrol = true;
            agent.SetDestination(pointToGo);

            while (isOnGoOnPatrol)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoOnPatrol = false;

                yield return waitForSeconds;
            }
            destinationForPatrolIndex = (destinationForPatrolIndex + 1) % pointsForPatrol.Length;
        }
    }
}
