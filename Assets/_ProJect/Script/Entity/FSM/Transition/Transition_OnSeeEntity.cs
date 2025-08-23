using UnityEngine;

public class Transition_OnSeeEntity : AbstractTransition
{
    public enum Condition { CheckForAllie, CheckForTarget, FollowAllied }

    [Header("Setting Transition_OnSeeEntity")]
    [SerializeField] private float viewAngleForward = 60;
    [SerializeField] private float viewAngleBack = 120;

    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float sightSenseDistance = 4;

    [SerializeField] private int raySightToAdd = 70;
    [SerializeField] private int raySenseToAdd = 30;


    [SerializeField] private Condition whatToCheck;

    [SerializeField] private float hight = 1;

    private bool isCheckForAllie;
    private bool isCheckForTarget;
    private bool isFollowAllied;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) 
    {
        //Davanti
        if(Utility.OnSeeOrSenseTarget(controller, hight, raySightToAdd, sightDistance, viewAngleForward, transform.forward, isCheckForTarget, isCheckForAllie, isFollowAllied, Color.yellow)) return true;

        //Dietro
        if(Utility.OnSeeOrSenseTarget(controller, hight, raySenseToAdd, sightSenseDistance, viewAngleBack, -transform.forward, isCheckForTarget, isCheckForAllie, isFollowAllied, Color.green)) return true;

        return false;
    }

    private void Start() => SwichtCondition();

    private void SwichtCondition()
    {
        isCheckForAllie = false;
        isCheckForTarget = false;
        isFollowAllied = false;

        switch (whatToCheck)
        {
            case Condition.CheckForAllie:
                isCheckForAllie = true; 
                break;
            case Condition.CheckForTarget:
                isCheckForTarget = true;
                break;
            case Condition.FollowAllied:
                isFollowAllied = true;
                break;
        }
    }
}
