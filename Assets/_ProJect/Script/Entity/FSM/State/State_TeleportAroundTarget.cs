    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_TeleportAroundTarget : AbstractState
{
    // Test Boss Non so se Lo faro
    [SerializeField] private Transform[] pointsForPatrol;

    private NavMeshPath pathToFollw;
    private NavMeshAgent agent;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State TeleportAroundTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();
        if (pathToFollw == null) pathToFollw = new NavMeshPath();

        agent.ResetPath();
        StartCoroutine(TeleportAroundTargetRoutine());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State TeleportAroundTarget");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator TeleportAroundTargetRoutine()
    {
        Vector3 newPosition = Utility.RandomPoint(agent,controller.Target.position,10,10);

        Vector3 currentPosition = agent.transform.position;
        agent.updatePosition = false;


        float distanceToTarget = Vector3.Distance(currentPosition, newPosition);
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * 20 / distanceToTarget;
            agent.transform.position = Vector3.Lerp(currentPosition, newPosition, progress);

            yield return null;
        }

        agent.updatePosition = true;
        agent.Warp(newPosition);
    }
}
