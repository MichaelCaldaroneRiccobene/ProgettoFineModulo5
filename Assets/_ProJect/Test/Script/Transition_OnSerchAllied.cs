using UnityEngine;

public class Transition_SerchAllied : AbstractTransition
{

    [SerializeField] private float viewAngleForward = 60;

    [SerializeField] private float sightDistance = 12;

    [SerializeField] private int raySightToAdd = 70;
    [SerializeField] private float hight = 1;

    [SerializeField] private bool isCheckForAllie;
    [SerializeField] private bool isCheckForTarget;
    [SerializeField] private bool isFollowAllied;
    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState)
    {
        if(Utility.OnSeeOrSenseTarget(controller, hight, raySightToAdd, sightDistance, viewAngleForward, transform.forward, isCheckForTarget, isCheckForAllie, isFollowAllied, Color.yellow))
        {
            controller.CanBeFollowTarget = true;
            return true;
        }

        return false;
    }

}
