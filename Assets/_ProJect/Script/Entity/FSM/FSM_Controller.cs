using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class FSM_Controller : MonoBehaviour, I_Team
{
    [Header("Setting")]
    [SerializeField] protected AbstractState defualtState;
    [SerializeField] protected float currentStateTime;
    [SerializeField] protected float updateTimerForTransition = 0.15f;

    [Header("Setting Team")]
    [SerializeField] protected int teamNumber;

    [SerializeField] protected bool canAttackFriend;
    [SerializeField] protected bool canBeAFollowTarget;

    [Header("Debug")]
    [SerializeField] protected bool canSeeDebug;
    [SerializeField] protected AbstractState currentState;
    [SerializeField] protected AbstractState[] subStates;

    protected AbstractState[] availableStates;
    protected AbstractState targetState;
    protected NavMeshAgent agent;

    protected Transform allied;
    protected Transform target;
    protected Transform lastTarget;

    public NavMeshAgent Agent => agent;
    public float CurrentStateTime => currentStateTime;

    public bool CanBeAFollowTarget { get => canBeAFollowTarget; set => canBeAFollowTarget = value; }
    public bool CanSeeDebug => canSeeDebug;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();   
        availableStates = GetComponentsInChildren<AbstractState>();

        foreach (AbstractState availableState in availableStates) availableState.SetUp(this);

        if (defualtState != null) SetUpState(defualtState);
        else SetUpState(availableStates[0]);

        StartCoroutine(EvaluateTransitionRoutine());
    }

    public virtual void Update()
    {
        if (currentState == null) return;

        currentStateTime += Time.deltaTime;
        currentState.StateUpdate();

        if (subStates != null)
        {
            foreach(AbstractState subState in subStates) subState.StateUpdate();
        }
    }

    public virtual void SetUpState(AbstractState state)
    {
        if(currentState != null)
        {
            foreach(AbstractState subState in subStates)
            {
                if (subState.gameObject.activeInHierarchy) subState.StateExit();
            }

            subStates = null;
        }

        currentStateTime = 0;
        currentState = state;

        subStates = currentState.GetComponentsInChildren<AbstractState>();
        foreach (AbstractState subState in subStates)
        {
            if(subState.gameObject.activeInHierarchy) subState.StateEnter();
        }
    }

    public virtual IEnumerator EvaluateTransitionRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(updateTimerForTransition);

        while (true)
        {
            targetState = currentState.EvaluateTransition();
            if (targetState != null) SetUpState(targetState);

            yield return waitForSeconds;
        }
    }

    #region I_Team

    public void ClearTarget() => target = null;
    public void ClearLastTarget() => lastTarget = null;
    public void ClearAllied() => allied = null;


    public void SetAllied(Transform allied) => this.allied = allied;
    public void SetTarget(Transform target) 
    {
        if (this.target != null) return;
        if (lastTarget != null) return;

        SetTargetForThis(target);
    }

    public void SetPriorityTarget(Transform target) => SetTargetForThis(target);
    public void SetLastTarget(Transform lastTarget) => SetTargetForThis(lastTarget);



    public int GetTeamNumber() => teamNumber;

    public Transform GetAllied() => allied;

    public Transform GetTarget() => target;
    public Transform GetLastTarget() => lastTarget;


    public bool CanBeFollow() => canBeAFollowTarget;

    public bool HasTarget() => target != null;

    private void SetTargetForThis(Transform target)
    {
        if (target == null) return;

        if (target.TryGetComponent(out I_Team entity))
        {
            if (entity.GetTeamNumber() == teamNumber)
            {
                if (!canAttackFriend)
                {
                    allied = target;
                }
                else
                {
                    this.target = target;
                    lastTarget = target;
                }
            }
            else
            {
                this.target = target;
                lastTarget = target;
            }
        }
    }
    #endregion
}
