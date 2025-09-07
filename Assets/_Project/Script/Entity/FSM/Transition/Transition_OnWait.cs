using UnityEngine;

public class Transition_OnWait : AbstractTransition
{
    [SerializeField] private float duration;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) => controller.CurrentStateTime >= duration;
}
