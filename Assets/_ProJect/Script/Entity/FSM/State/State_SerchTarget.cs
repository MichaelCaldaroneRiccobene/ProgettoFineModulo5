using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_SerchTarget : AbstractState
{
    [Header("Setting SerchTarget")]
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private NavMeshAgent agent;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State SerchTarget");
        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        StartCoroutine(GoOnSerchTargetRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State SerchTarget");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnSerchTargetRoutin()
    {
        if(controller.LastTarget == null) yield break;
        agent.stoppingDistance = stopDistanceToDestination;
        agent.ResetPath();

        while (controller.LastTarget)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

            Vector3 positionToFollow = Utility.RandomPoint(agent, controller.LastTarget.position, radiusRandomPosition);
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            agent.SetDestination(positionToFollow);
            while (agent.pathPending) yield return null;

            while (agent.remainingDistance > agent.stoppingDistance) { yield return waitForSeconds; }
            yield return null;

            controller.LastTarget = null;
        }
    }
}
