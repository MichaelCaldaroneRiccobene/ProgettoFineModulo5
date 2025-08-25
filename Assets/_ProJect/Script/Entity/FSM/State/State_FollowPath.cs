using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowPath : AbstractState
{
    [Header("Setting FollowPath")]
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
        agent.stoppingDistance = stopDistanceToDestination;

        agent.ResetPath();

        if (pointsForPatrol == null || pointsForPatrol.Length <= 0)
        {
            if(controller.CanSeeDebug) Debug.Log("No Path");
            yield break;
        }

        int destinationForPatrolIndex = 0;

        while (true)
        {
            if (agent.CalculatePath(pointsForPatrol[destinationForPatrolIndex].position, pathToFollw)) pointToGo = pointsForPatrol[destinationForPatrolIndex].position;

            agent.SetDestination(pointToGo);
            while (agent.pathPending) yield return null;

            while (agent.remainingDistance > agent.stoppingDistance) { yield return waitForSeconds; }

            destinationForPatrolIndex = (destinationForPatrolIndex + 1) % pointsForPatrol.Length;

            yield return null;
        }
    }
}
