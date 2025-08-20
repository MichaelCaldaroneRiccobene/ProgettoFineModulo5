using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_Controller : MonoBehaviour, I_Team
{
    [SerializeField] private AbstractState defualtState;
    [SerializeField] private float currentStateTime;

    [SerializeField] private AbstractState currentState;

    [SerializeField] private float timeUpdateEvaluateTransition = 0.15f;
    [SerializeField] private int teamNumber;


    public Transform Allied;
    public Transform Target;
    public Transform LastTarget;

    public FSM_Controller(Transform lastTarget)
    {
        LastTarget = lastTarget;
    }

    public bool CanBeFollowTarget;
    public bool CanSeeDebug;

    private AbstractState targetState;

    public float TimeUpdateEvaluateTransition => timeUpdateEvaluateTransition;

    public int TeamNumber => teamNumber;

    public float CurrentStateTime => currentStateTime;

    private void Start()
    {
        AbstractState[] availableStates = GetComponentsInChildren<AbstractState>(true);

        foreach (AbstractState availableState in availableStates) availableState.SetUp(this);

        if(defualtState != null) SetUpState(defualtState);
        else SetUpState(availableStates[0]);

        StartCoroutine(EvaluateTransitionRoutine());
    }

    private void Update()
    {
        if (currentState != null)
        {
            currentStateTime += Time.deltaTime;
            currentState.StateUpdate();
        }
    }

    private IEnumerator EvaluateTransitionRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);

        while (true)
        {
            targetState = currentState.EvaluateTransition();
            if (targetState != null) SetUpState(targetState);

            yield return waitForSeconds;
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

    public void SetTarget(Transform target)
    {
        if (LastTarget == null) Target = target;
    }

    public void SetTargetPriority(Transform target)
    {
        Target = target;
        LastTarget = target;
    }

    public Transform GetAllied() => Allied;

    public bool HasTarget() => Target != null;

    public bool CanBeFollow() => CanBeFollowTarget;

    public void OnDead() => Destroy(gameObject);
}
