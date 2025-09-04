using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum WhoFollow
{
    None = 0,Allied = 1,Enemy = 2,
}

public class State_FollowEntity : AbstractState
{
    [Header("Setting FollowEntity")]
    [SerializeField] private WhoFollow whoFollow;
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private float radiusForPosition;
    [SerializeField] private bool isOnRandomSpot;

    private bool isFollowATarget;
    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State FollowEntity");

        SelectWhoFollow();
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State FollowEntity");

        StopAllCoroutines();
        controller.CanBeAFollowTarget = false;
        controller.Agent.ResetPath();
    }

    public override void StateUpdate() { }


    private void SelectWhoFollow()
    {
        isFollowATarget = false;
        switch (whoFollow)
        {
            case WhoFollow.None:
                break;
            case WhoFollow.Allied:
                controller.CanBeAFollowTarget = true;
                StartCoroutine(GoOnEntityRoutin());
                break;
            case WhoFollow.Enemy:
                isFollowATarget = true;
                StartCoroutine(GoOnEntityRoutin());
                break;
        }
    }

    private IEnumerator GoOnEntityRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        controller.Agent.stoppingDistance = stopDistanceToDestination;
        controller.Agent.ResetPath();

        while (true)
        {
            Transform followTarget = isFollowATarget ? controller.GetTarget() : controller.GetAllied();
            if(followTarget == null) break;

            Vector3 positionToFollow = isOnRandomSpot? Utility.RandomPoint(controller.Agent, followTarget.position, radiusForPosition) : followTarget.position;
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            controller.Agent.SetDestination(positionToFollow);
            while (controller.Agent.pathPending) yield return null;

            if(!isFollowATarget)  while (controller.Agent.remainingDistance > controller.Agent.stoppingDistance) yield return waitForSeconds; 

            yield return waitForSeconds;
        }
    }
}
