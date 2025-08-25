using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowEnemyTarget : AbstractState
{
    [Header("Setting FollowEnemyTarget")]
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private float hight = 1;

    [SerializeField] private int rayToAdd = 100;
    [SerializeField] private float viewAngleBack = 180;
    [SerializeField] private float sightDistance = 12;

    [SerializeField] private bool isCheckForAllie;

    private NavMeshAgent agent;
    private float distanceToTarget;

    public event Action OnTryMeleeAttack;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowTarget");
        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        StartCoroutine(GoOnTargetRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowTarget");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { LookOnTarget(); }

    private IEnumerator GoOnTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        agent.stoppingDistance = stopDistanceToDestination;

        agent.ResetPath();

        while (controller.Target != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, controller.Target.position);

            if (isCheckForAllie)
            {
                Utility.OnSeeOrSenseTarget(controller, hight, rayToAdd, sightDistance, viewAngleBack, transform.forward, false, isCheckForAllie, false, Color.blue);
            }


            if (distanceToTarget > agent.stoppingDistance)
            {
                agent.SetDestination(controller.Target.position);
                while (agent.pathPending) yield return null;

                yield return waitForSeconds;
            }
            else
            {
                OnTryMeleeAttack?.Invoke();
                agent.ResetPath();
            }

            yield return null;
        }

    }

    private void LookOnTarget()
    {
        if (controller.Target == null && distanceToTarget > agent.stoppingDistance) return;

        Quaternion lookDirection = Quaternion.LookRotation((controller.Target.position - agent.transform.position).normalized);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, lookDirection, Time.deltaTime * rotationSpeed);
    }
}
