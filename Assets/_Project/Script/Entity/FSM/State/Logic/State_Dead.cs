using System;
using System.Collections;
using UnityEngine;

public class State_Dead : AbstractState
{
    [SerializeField] private GameObject objToDestroy;
    [SerializeField] private float timeForDestroy = 5;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State Dead");

        StartCoroutine(DeastoyOnDeadRoutine());
    }

    public override void StateExit() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Dead"); }

    public override void StateUpdate() { }

    private IEnumerator DeastoyOnDeadRoutine()
    {
        yield return new WaitForSeconds(timeForDestroy);

        Destroy(objToDestroy);
    }
}
