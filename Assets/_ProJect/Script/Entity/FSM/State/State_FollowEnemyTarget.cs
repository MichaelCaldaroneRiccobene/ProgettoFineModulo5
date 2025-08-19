using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using static Transition_OnSeeEntity;

public class State_FollowEnemyTarget : AbstractState
{
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float hight = 1;

    [SerializeField] private int rayToAdd = 100;
    [SerializeField] private float viewAngleBack = 180;
    [SerializeField] private float sightDistance = 12;

    [SerializeField] private bool isCheckForAllie;

    private NavMeshAgent agent;
    private NavMeshPath pathToFollw;

    public event Action OnTryMeleeAttack;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();
        if (pathToFollw == null) pathToFollw = new NavMeshPath();

        agent.ResetPath();
        agent.updateRotation = false;
        StartCoroutine(GoOnTargetRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowTarget");

        StopAllCoroutines();
        agent.ResetPath();
        agent.updateRotation = true;
    }

    public override void StateUpdate() { LookOnTarget(); }

    private IEnumerator GoOnTargetRoutin()
    {
        if (controller.Target != null)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
            agent.ResetPath();

            while (controller.Target != null)
            {
                if (agent.CalculatePath(controller.Target.position, pathToFollw))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, controller.Target.position);

                    if (distanceToTarget > stopDistanceToDestination) agent.destination = controller.Target.position;
                    else
                    {
                        OnTryMeleeAttack?.Invoke();
                        agent.ResetPath();
                    }
                }
                if(isCheckForAllie)
                {
                    Utility.OnSeeOrSenseTarget(controller, hight, rayToAdd, sightDistance, viewAngleBack, transform.forward, false, isCheckForAllie, false, Color.blue);
                }
                yield return waitForSeconds;
            }
        }
    }

    private void LookOnTarget()
    {
        if (controller.Target == null) return;

        Quaternion lookDirection = Quaternion.LookRotation((controller.Target.position - agent.transform.position).normalized);

        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, lookDirection, Time.deltaTime * rotationSpeed);
    }
}
