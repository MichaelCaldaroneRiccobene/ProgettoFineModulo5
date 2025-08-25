using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowAlliedTarget : AbstractState
{
    [Header("Setting FollowAlliedTarget")]
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private float radiusForPosition;
    [SerializeField] private bool isOnRandomSpot;

    private NavMeshAgent agent;

    public override void StateEnter()
    {
        if(controller.CanSeeDebug) Debug.Log("Entrato in State FollowAlliedTarget");
        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        controller.CanBeAFollowTarget = true;
        StartCoroutine(GoOnTargetRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowAlliedTarget");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        agent.stoppingDistance = stopDistanceToDestination;
        agent.ResetPath();

        while (controller.Allied != null)
        {
            Vector3 positionToFollow = isOnRandomSpot ? Utility.RandomPoint(agent, controller.Allied.transform.position, radiusForPosition) : controller.Allied.transform.position;
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            agent.SetDestination(positionToFollow);
            while (agent.pathPending) yield return null;

            while (agent.remainingDistance > agent.stoppingDistance) { yield return waitForSeconds; }

            yield return null;
        }
    }
}
