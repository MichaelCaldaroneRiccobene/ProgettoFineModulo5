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
    private Vector3 pointToGo;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowPath");
        if (pathToFollw == null) pathToFollw = new NavMeshPath();

        StartCoroutine(GoOnPatrolRoutine());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowPath");

        StopAllCoroutines();
        controller.Agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnPatrolRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
        controller.Agent.stoppingDistance = stopDistanceToDestination;

        controller.Agent.ResetPath();

        if (pointsForPatrol == null || pointsForPatrol.Length <= 0)
        {
            if (controller.CanSeeDebug) Debug.Log("No Path");
            yield break;
        }

        int destinationForPatrolIndex = 0;

        while (true)
        {
            pointToGo = pointsForPatrol[destinationForPatrolIndex].position;
            while (controller.Agent.pathPending) yield return null;

            controller.Agent.SetDestination(pointToGo);
            while (controller.Agent.remainingDistance > controller.Agent.stoppingDistance) { yield return waitForSeconds; }

            destinationForPatrolIndex = (destinationForPatrolIndex + 1) % pointsForPatrol.Length;

            yield return null;
        }
    }
}
