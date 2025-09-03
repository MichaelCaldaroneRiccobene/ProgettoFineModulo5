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
        switch (whoFollow)
        {
            case WhoFollow.None:
                break;
            case WhoFollow.Allied:
                controller.CanBeAFollowTarget = true;
                StartCoroutine(GoOnAlliedRoutin());
                break;
            case WhoFollow.Enemy:
                StartCoroutine(GoOnTaregetRoutin());
                break;
        }
    }

    // (Ho Creato 2 funzioni quasi identiche perchè se si cambiava target, Ai andava sempre sul vecchio).
    private IEnumerator GoOnTaregetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        controller.Agent.stoppingDistance = stopDistanceToDestination;
        controller.Agent.ResetPath();

        while (controller.GetTarget() != null)
        {
            Vector3 positionToFollow = isOnRandomSpot ? Utility.RandomPoint(controller.Agent, controller.GetTarget().position, radiusForPosition) : controller.GetTarget().position;
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            controller.Agent.SetDestination(positionToFollow);
            while (controller.Agent.pathPending) yield return null;

            yield return waitForSeconds;
        }
    }

    private IEnumerator GoOnAlliedRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        controller.Agent.stoppingDistance = stopDistanceToDestination;
        controller.Agent.ResetPath();

        while (controller.GetAllied() != null)
        {
            Vector3 positionToFollow = isOnRandomSpot ? Utility.RandomPoint(controller.Agent, controller.GetAllied().position, radiusForPosition) : controller.GetAllied().position;
            if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

            controller.Agent.SetDestination(positionToFollow);
            while (controller.Agent.pathPending) yield return null;

            while(controller.Agent.remainingDistance > controller.Agent.stoppingDistance) yield return waitForSeconds;
            yield return null; 
        }
    }
}
