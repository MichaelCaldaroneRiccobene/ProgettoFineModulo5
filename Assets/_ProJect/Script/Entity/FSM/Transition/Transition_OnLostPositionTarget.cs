
public class Transition_OnLostPositionTarget : AbstractTransition
{
    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) => controller.LastTarget == null;
}
