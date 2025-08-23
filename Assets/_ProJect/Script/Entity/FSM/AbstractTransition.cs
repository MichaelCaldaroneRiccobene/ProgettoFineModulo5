using UnityEngine;

public abstract class AbstractTransition : MonoBehaviour
{
    [Header("Next State")]
    [SerializeField] protected AbstractState targetState;

    public AbstractState GetTargetState() => targetState;

    public abstract bool IsConditionMet(FSM_Controller controller, AbstractState ownerState);
}
