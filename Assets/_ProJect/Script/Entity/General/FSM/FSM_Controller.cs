using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class FSM_Controller : MonoBehaviour, I_Team
{
    [SerializeField] private AbstractState defualtState;
    [SerializeField] private float currentStateTime;

    [SerializeField] private AbstractState currentState;

    [SerializeField] private int teamNumber;


    public int TeamNumber => teamNumber;
    public Transform Target;
    public Transform LastTarget;

    public float CurrentStateTime => currentStateTime;

    private void Start()
    {
        AbstractState[] availableStates = GetComponentsInChildren<AbstractState>(true);

        foreach (AbstractState availableState in availableStates) availableState.SetUp(this);

        if(defualtState != null) SetUpState(defualtState);
        else SetUpState(availableStates[0]);
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentStateTime += Time.deltaTime;
            currentState.StateUpdate();

            AbstractState targetState = currentState.EvaluateTransition();

            if(targetState != null) SetUpState(targetState);
        }
    }

    private void SetUpState(AbstractState state)
    {
        if (currentState != null) currentState.StateExit();

        currentStateTime = 0;
        currentState = state;
        currentState.StateEnter();
    }

    public int GetTeamNumber() => teamNumber;

    public void SetTarget(Transform target) => Target = target;
}
