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

    public event Action OnTryMeleeAttack;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

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

        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);

        while (controller.Target != null)
        {
            float distanceToTarget = Vector3.Distance(transform.position, controller.Target.position);

            if (distanceToTarget >= stopDistanceToDestination)
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


            if (isCheckForAllie)
            {
                Utility.OnSeeOrSenseTarget(controller, hight, rayToAdd, sightDistance, viewAngleBack, transform.forward, false, isCheckForAllie, false, Color.blue);
            }
            yield return null;
        }

    }

    private void LookOnTarget()
    {
        if (controller.Target == null) return;

        Quaternion lookDirection = Quaternion.LookRotation((controller.Target.position - agent.transform.position).normalized);
        agent.transform.rotation = Quaternion.Lerp(agent.transform.rotation, lookDirection, Time.deltaTime * rotationSpeed);
    }
}
