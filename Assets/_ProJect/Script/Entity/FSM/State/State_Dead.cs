using System;
using System.Collections;
using UnityEngine;

public class State_Dead : AbstractState
{
    [SerializeField] private GameObject objToDestroy;

    public event Action OnTriggerDead;

    public override void StateEnter() 
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Dead");

        OnTriggerDead?.Invoke();
        StartCoroutine(DeastoyOnDeadRoutine());
    }

    public override void StateExit() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Dead"); }

    public override void StateUpdate() { }

    private IEnumerator DeastoyOnDeadRoutine()
    {
        yield return new  WaitForSeconds(5);

        Destroy(objToDestroy);
    }
}
