public class Transition_OnDead : AbstractTransition
{
    private LifeSistem lifeSistem;

    private void Start() => lifeSistem = GetComponentInParent<LifeSistem>();
    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState)
    {
        if (lifeSistem != null && lifeSistem.IsDead()) return true;
        else return false;
    }
}
    