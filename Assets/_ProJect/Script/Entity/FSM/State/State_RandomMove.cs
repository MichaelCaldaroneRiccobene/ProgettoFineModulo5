using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class State_RandomMove : AbstractState
{
    [Header("Setting RandomMove")]
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float radiusRandomPosition = 10;
    [SerializeField] private float stopDistanceToDestination = 2f;

    private NavMeshAgent agent;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State RandomMove");
        if(agent == null) agent = GetComponentInParent<NavMeshAgent>();

        agent.ResetPath();
        StartCoroutine(GoOnRandomPointRoutin());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State RandomMove");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnRandomPointRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        while (true)
        {
            Vector3 positionToFollow = Utility.RandomPoint(agent, agent.transform.position, radiusRandomPosition);
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            agent.SetDestination(positionToFollow);
            while (agent.pathPending) yield return null;

            while (agent.remainingDistance > stopDistanceToDestination) { yield return waitForSeconds; }
        }
    }
}
