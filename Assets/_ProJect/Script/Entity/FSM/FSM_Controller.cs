using System.Collections;
using UnityEngine;

public class FSM_Controller : MonoBehaviour, I_Team
{
    [Header("Setting")]
    [SerializeField] protected AbstractState defualtState;

    [SerializeField] protected float currentStateTime;
    [SerializeField] protected float timeUpdateEvaluateTransition = 0.15f;

    [Header("Setting Team")]
    [SerializeField] protected int teamNumber;

    [SerializeField] protected bool canAttackFriend;
    [SerializeField] protected bool canBeFollowTarget;

    [Header("Debug")]
    [SerializeField] protected bool canSeeDebug;
    [SerializeField] protected AbstractState currentState;

    public Transform Allied;
    public Transform Target;
    public Transform LastTarget;

    protected AbstractState targetState;

    public float CurrentStateTime => currentStateTime;
    public float TimeUpdateEvaluateTransition => timeUpdateEvaluateTransition;

    public int TeamNumber => teamNumber;

    public bool CanBeFollowTarget { get => canBeFollowTarget; set => value = canBeFollowTarget;}
    public bool CanSeeDebug => canSeeDebug;

    public virtual void Start()
    {
        AbstractState[] availableStates = GetComponentsInChildren<AbstractState>(true);

        foreach (AbstractState availableState in availableStates) availableState.SetUp(this);

        if(defualtState != null) SetUpState(defualtState);
        else SetUpState(availableStates[0]);

        StartCoroutine(EvaluateTransitionRoutine());
    }

    public virtual void Update()
    {
        if (currentState != null)
        {
            currentStateTime += Time.deltaTime;
            currentState.StateUpdate();
        }
    }

    public virtual IEnumerator EvaluateTransitionRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.15f);

        while (true)
        {
            targetState = currentState.EvaluateTransition();
            if (targetState != null) SetUpState(targetState);

            yield return waitForSeconds;
        }
    }

    public virtual void SetUpState(AbstractState state)
    {
        if (currentState != null) currentState.StateExit();

        currentStateTime = 0;
        currentState = state;
        currentState.StateEnter();
    }

    #region I_Team
    public virtual void SetTarget(Transform target) { if (LastTarget == null) SetTargetForMe(target); }
    public virtual void SetPriorityTarget(Transform target) => SetTargetForMe(target);

    public virtual void SetTargetForMe(Transform target)
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

    public virtual int GetTeamNumber() => teamNumber;
    public virtual Transform GetAllied() => Allied;

    public virtual bool CanBeFollow() => CanBeFollowTarget;
    public virtual bool HasTarget() => Target != null;
    #endregion

    public virtual void OnDead() => Destroy(gameObject);
}
