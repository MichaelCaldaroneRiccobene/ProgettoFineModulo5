using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_SerchTarget : AbstractState
{
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private NavMeshAgent agent;
    private Vector3 pointToGo;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State SerchTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        agent.ResetPath();
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

        while (controller.LastTarget)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
            bool isOnGoRandomPoint = false;

            Utility.RandomPoint(agent, controller.LastTarget.position, radiusRandomPosition, out pointToGo);
            agent.SetDestination(pointToGo);

            while (agent.pathPending) yield return null;

            isOnGoRandomPoint = true;

            while (isOnGoRandomPoint)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoRandomPoint = false;

                yield return waitForSeconds;
            }
            controller.LastTarget = null;
        }
    }
}
