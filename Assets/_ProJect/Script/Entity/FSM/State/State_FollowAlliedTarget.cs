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

        controller.CanBeFollowTarget = true;
        agent.ResetPath();
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
        agent.ResetPath();

        while (controller.Allied != null)
        {
            Vector3 positionToFollow = isOnRandomSpot ? Utility.RandomPoint(agent, controller.Allied.transform.position, radiusForPosition) : controller.Allied.transform.position;
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            if(isOnRandomSpot)
            {
                agent.SetDestination(positionToFollow);
                while (agent.pathPending) yield return null;

                while (agent.remainingDistance > stopDistanceToDestination) { yield return waitForSeconds; }
            }
            else
            {
                float distanceToTarget = Vector3.Distance(transform.position, positionToFollow);

                if (distanceToTarget >= stopDistanceToDestination)
                {
                    agent.SetDestination(positionToFollow);
                    while (agent.pathPending) yield return null;

                    yield return waitForSeconds;
                }
                else agent.ResetPath();

            }
            yield return null;
        }
        agent.ResetPath();
    }
}
