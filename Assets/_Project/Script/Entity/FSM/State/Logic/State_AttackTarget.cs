using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class State_AttackTarget : AbstractState
{
    [SerializeField] private float timeUpdateSightRoutine = 0.1f;
    [SerializeField] private float rotationSpeed = 10;

    public UnityEvent OnAttack;

    private float distanceToTarget;

    public override void StateEnter() 
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State AttackTarget");

        StartCoroutine(AttackRoutine());
    }

    public override void StateUpdate() { LookOnTarget(); }

    public override void StateExit() 
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State AttackTarget");

        StopAllCoroutines();
    }

    private IEnumerator AttackRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);

        while (controller.GetTarget() != null)
        {
            distanceToTarget = Vector3.Distance(transform.position, controller.GetTarget().position) - 1;

            if (distanceToTarget < controller.Agent.stoppingDistance) OnAttack?.Invoke();

            yield return waitForSeconds;
        }
    }
    private void LookOnTarget()
    {
        if (controller.GetTarget() == null || distanceToTarget > controller.Agent.stoppingDistance) return;

        Quaternion lookDirection = Quaternion.LookRotation((controller.GetTarget().position - controller.Agent.transform.position).normalized);
        controller.Agent.transform.rotation = Quaternion.Lerp(controller.Agent.transform.rotation, lookDirection, Time.deltaTime * rotationSpeed);
    }
}
