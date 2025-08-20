using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowAlliedTarget : AbstractState
{
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private float radiusForPosition;
    [SerializeField] private bool isOnRandomSpot;


    private float timeForChangePosition = 5;
    private float timerForForChangePosition;

    private NavMeshAgent agent;
    private NavMeshPath pathToFollw;

    public override void StateEnter()
    {
        if(controller.CanSeeDebug) Debug.Log("Entrato in State FollowAlliedTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();
        if (pathToFollw == null) pathToFollw = new NavMeshPath();

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
        bool forceExitWhile = false;
        agent.ResetPath();

        while (controller.Allied != null)
        {
            Vector3 positionToFollow = isOnRandomSpot ? Utility.RandomPoint(agent, controller.Allied.transform.position, radiusForPosition) : controller.Allied.transform.position;
            timerForForChangePosition = 0;
            forceExitWhile = false;

            if (agent.CalculatePath(positionToFollow, pathToFollw))
            {
                float distanceToTarget = Vector3.Distance(transform.position, positionToFollow);

                if (distanceToTarget >= stopDistanceToDestination)
                {
                    agent.destination = positionToFollow;

                    while (!forceExitWhile && distanceToTarget >= stopDistanceToDestination)
                    {
                        distanceToTarget = Vector3.Distance(transform.position, positionToFollow);
                        timerForForChangePosition += timeUpdateSightRoutine;

                        if (timerForForChangePosition >= timeForChangePosition) forceExitWhile = true;

                        yield return waitForSeconds;
                    }
                }
                else agent.ResetPath();
            }
            yield return waitForSeconds;

        }

        agent.ResetPath();
    }
}
