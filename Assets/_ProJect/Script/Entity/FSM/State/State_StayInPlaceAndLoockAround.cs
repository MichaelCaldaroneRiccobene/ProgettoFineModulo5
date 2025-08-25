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

    public UnityEvent OnTurn180;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private NavMeshAgent agent;
    private bool isStartSetUpPositionAndRotation;


    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State StayInPlaceAndLoockAround");
        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        if(!isStartSetUpPositionAndRotation)
        {
            isStartSetUpPositionAndRotation = true;

            startPosition = transform.position;
            startRotation = transform.rotation;
        }

        agent.ResetPath();
        StartCoroutine(GoOnStartPosition());
    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State StayInPlaceAndLoockAround");

        StopAllCoroutines();
        agent.ResetPath();
        agent.updateRotation = true;
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnStayInPlaceAndLoockAroundRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeForStayOnPlaceAndLookAround);
        agent.ResetPath();

        while (true)
        {
            Quaternion startRotation = agent.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(-transform.forward,Vector3.up);

            OnTurn180?.Invoke();
            float progress = 0;

            while (progress < 1)
            {
                progress += Time.deltaTime;
                agent.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);

                yield return null;
            }

            yield return waitForSeconds;
        }
    }

    private IEnumerator GoOnStartPosition()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);
        agent.stoppingDistance = stopDistanceToDestination;

        Vector3 positionToFollow = startPosition;
        if (NavMesh.SamplePosition(positionToFollow, out NavMeshHit hit, 2f, NavMesh.AllAreas)) positionToFollow = hit.position;

        agent.SetDestination(positionToFollow);
        while (agent.pathPending) yield return null;

        while (agent.remainingDistance > agent.stoppingDistance) { yield return waitForSeconds; }


        agent.updateRotation = false;
        Quaternion curretRotation = agent.transform.rotation;
        float velocityRotation = 10;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * velocityRotation;
            agent.transform.rotation = Quaternion.Lerp(curretRotation, startRotation, progress);

            yield return null;
        }
        agent.updateRotation = true;

        StartCoroutine(GoOnStayInPlaceAndLoockAroundRoutine());
    }
}
