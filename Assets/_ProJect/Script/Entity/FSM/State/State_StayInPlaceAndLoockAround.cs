using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class State_StayInPlaceAndLoockAround : AbstractState
{
    [SerializeField] private float timeForStayOnPlaceAndLookAround = 2f;
    [SerializeField] private float timeUpdateRoutine = 1f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    public event Action OnTurn180;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private NavMeshAgent agent;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State StayInPlaceAndLoockAround");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        if (startPosition == null) startPosition = controller.transform.position;
        if (startRotation == null) startRotation = controller.transform.rotation;

        agent.ResetPath();
        StartCoroutine(GoOnStayInPlaceAndLoockAroundRoutine());


    }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State StayInPlaceAndLoockAround");

        StopAllCoroutines();
        agent.ResetPath();
    }

    public override void StateUpdate() { }

    private IEnumerator GoOnStayInPlaceAndLoockAroundRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeForStayOnPlaceAndLookAround);
        agent.ResetPath();

        while (true)
        {
            Quaternion startRotation = agent.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward * 10 * -1);

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

        agent.SetDestination(startPosition);
        while (agent.pathPending) yield return null;

        bool isOnStartPosition = false;
        while (!isOnStartPosition)
        {
            if (agent.remainingDistance < stopDistanceToDestination) isOnStartPosition = true;

            yield return waitForSeconds;
        }

        bool isOnStartRotation = false;
        while (!isOnStartRotation)
        {
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
            isOnStartRotation = true;
            agent.updateRotation = true;
        }

        StartCoroutine(GoOnStayInPlaceAndLoockAroundRoutine());
    }
}
