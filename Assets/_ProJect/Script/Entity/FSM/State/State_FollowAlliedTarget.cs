using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowAlliedTarget : AbstractState
{
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

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
        if (controller.Allied != null)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
            agent.ResetPath();

            while (controller.Allied != null)
            {
                if (agent.CalculatePath(controller.Allied.position, pathToFollw))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, controller.Allied.position);

                    if (distanceToTarget > stopDistanceToDestination) agent.destination = controller.Allied.position;
                    else agent.ResetPath();
                }
                yield return waitForSeconds;
            }
        }
    }
}
