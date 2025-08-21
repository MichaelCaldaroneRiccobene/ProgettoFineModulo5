using System.Collections;
using UnityEngine;

public class FSM_Controller : MonoBehaviour, I_Team
{
    [Header("Setting")]
    [SerializeField] private AbstractState defualtState;

    [SerializeField] private float currentStateTime;
    [SerializeField] private float timeUpdateEvaluateTransition = 0.15f;

    [Header("Setting Team")]
    [SerializeField] private int teamNumber;

    [SerializeField] private bool canAttackFriend;
    [SerializeField] private bool canBeFollowTarget;

    [Header("Debug")]
    [SerializeField] private bool canSeeDebug;
    [SerializeField] private AbstractState currentState;

    public Transform Allied;
    public Transform Target;
    public Transform LastTarget;

    private AbstractState targetState;

    public float CurrentStateTime => currentStateTime;
    public float TimeUpdateEvaluateTransition => timeUpdateEvaluateTransition;

    public int TeamNumber => teamNumber;

    public bool CanBeFollowTarget { get => canBeFollowTarget; set => value = canBeFollowTarget;}
    public bool CanSeeDebug => canSeeDebug;

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

    #region I_Team
    public void SetTarget(Transform target) { if (LastTarget == null) SetTargetForMe(target); }
    public void SetPriorityTarget(Transform target) => SetTargetForMe(target);

    private void SetTargetForMe(Transform target)
    {
        if (target.TryGetComponent(out I_Team entity))
        {
            if (entity.GetTeamNumber() != teamNumber)
            {
                Target = target;
                LastTarget = target;

                return;
            }
            else if (canAttackFriend)
            {
                Target = target;
                LastTarget = target;

                return;
            }
        }
    }

    public int GetTeamNumber() => teamNumber;
    public Transform GetAllied() => Allied;

    public bool CanBeFollow() => CanBeFollowTarget;
    public bool HasTarget() => Target != null;
    #endregion

    public void OnDead() => Destroy(gameObject);
}
