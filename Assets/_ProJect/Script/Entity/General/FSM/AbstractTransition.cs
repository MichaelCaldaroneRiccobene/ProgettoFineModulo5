using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbstractTransition : MonoBehaviour
{
    [SerializeField] protected AbstractState targetState;

    public AbstractState GetTargetState() => targetState;

    public abstract bool IsConditionMet(FSM_Controller controller, AbstractState ownerState);
}
