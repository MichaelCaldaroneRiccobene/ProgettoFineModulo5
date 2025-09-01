using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_OnFindEntity : AbstractTransition
{
    public enum WhoFollow
    {
        None = 0, FollowTarget = 1, FollowAllied = 2
    }

    [SerializeField] private WhoFollow whoFollow;

    private bool isFollowTarget;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) => FollowEntity(controller);

    private void Start()
    {
        isFollowTarget = false;

        switch (whoFollow)
        {
            case WhoFollow.None:
                break;
            case WhoFollow.FollowTarget:
                isFollowTarget = true;
                break;
            case WhoFollow.FollowAllied:
                isFollowTarget = false;
                break;
        }
    }

    private bool FollowEntity(FSM_Controller controller)
    {
        if(isFollowTarget)
        {
            if (controller.GetTarget() != null) return true;
            else return false;
        }
        else
        {
            if (controller.GetAllied() != null) return true;
            else return false;
        }
    }
}
