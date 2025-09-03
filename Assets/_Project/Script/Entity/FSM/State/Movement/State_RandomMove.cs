using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_RandomMove : AbstractState
{
    [Header("Setting RandomMove")]
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State RandomMove");

        controller.Agent.ResetPath();
        StartCoroutine(GoOnRandomPointRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State RandomMove");

        StopAllCoroutines();
        controller.Agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnRandomPointRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
        controller.Agent.stoppingDistance = stopDistanceToDestination;
        yield return null;

        while (true)
        {
            Vector3 positionToFollow = Utility.RandomPoint(controller.Agent, controller.Agent.transform.position, radiusRandomPosition);
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            controller.Agent.SetDestination(positionToFollow);
            while (controller.Agent.pathPending) yield return null;

            while (controller.Agent.remainingDistance > controller.Agent.stoppingDistance) { yield return waitForSeconds; }

            yield return waitForSeconds;
        }
    }
}
