
public class Transition_OnLostPositionTarget : AbstractTransition
{
    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState)
    {
        if (controller.GetLastTarget() == null) return true;
        else return false;
    }
}
