using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_SerchTarget : AbstractState
{
    [Header("Setting SerchTarget")]
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State SerchTarget");

        StartCoroutine(GoOnSerchTargetRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State SerchTarget");

        StopAllCoroutines();
        controller.Agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnSerchTargetRoutin()
    {
        if (controller.GetLastTarget() == null) yield break;
        controller.Agent.stoppingDistance = stopDistanceToDestination;
        controller.Agent.ResetPath();

        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        Vector3 positionToFollow = Utility.RandomPoint(controller.Agent, controller.GetLastTarget().position, radiusRandomPosition);
        if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

        controller.Agent.SetDestination(positionToFollow);
        while (controller.Agent.pathPending) yield return null;

        while (controller.Agent.remainingDistance > controller.Agent.stoppingDistance) { yield return waitForSeconds; }
        yield return null;
         
        controller.ClearLastTarget();
    }
}
