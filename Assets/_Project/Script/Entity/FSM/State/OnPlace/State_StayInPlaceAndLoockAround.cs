using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class State_StayInPlaceAndLoockAround : AbstractState
{
    [Header("Setting StayInPlaceAndLoockAround")]
    [SerializeField] private float timeForStayOnPlaceAndLookAround = 2f;
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private bool randomTimeForTurn = true;

    public UnityEvent OnTurn180;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private bool isStartSetUpPositionAndRotation;

    private float timeRotation;
    private float minTimeRotation = 3f;
    private float maxTimeRoation = 7f;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State StayInPlaceAndLoockAround");

        if(!isStartSetUpPositionAndRotation)
        {
            isStartSetUpPositionAndRotation = true;

            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        timeRotation = randomTimeForTurn ? Random.Range(minTimeRotation, maxTimeRoation) : timeForStayOnPlaceAndLookAround;
        controller.Agent.ResetPath();
        StartCoroutine(GoOnStartPosition());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State StayInPlaceAndLoockAround");

        StopAllCoroutines();
        controller.Agent.ResetPath();
        controller.Agent.updateRotation = true;
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnStayInPlaceAndLoockAroundRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeRotation);
        controller.Agent.ResetPath();

        while (true)
        {
            Quaternion startRotation = controller.Agent.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(-transform.forward * 1);

            OnTurn180?.Invoke();
            float progress = 0;

            while (progress < 1)
            {
                progress += Time.deltaTime;
                controller.Agent.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);

                yield return null;
            }
            controller.Agent.transform.rotation = targetRotation;
            yield return waitForSeconds;
        }
    }

    private IEnumerator GoOnStartPosition()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
        controller.Agent.stoppingDistance = stopDistanceToDestination;

        Vector3 positionToFollow = startPosition;
        if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

        controller.Agent.SetDestination(positionToFollow);
        while (controller.Agent.pathPending) yield return null;

        while (controller.Agent.remainingDistance > controller.Agent.stoppingDistance) { yield return waitForSeconds; }


        controller.Agent.updateRotation = false;
        Quaternion curretRotation = controller.Agent.transform.rotation;
        float velocityRotation = 10;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * velocityRotation;
            controller.Agent.transform.rotation = Quaternion.Lerp(curretRotation, startRotation, progress);

            yield return null;
        }
        controller.Agent.updateRotation = true;
        StartCoroutine(GoOnStayInPlaceAndLoockAroundRoutine());
    }
}
