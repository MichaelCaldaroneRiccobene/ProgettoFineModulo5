using System.Collections;
using UnityEngine;

public class Transition_OnLostSightEntity : AbstractTransition
{
    public enum Condition {OnLostTarget, OnLostAllied }

    [SerializeField] private float hight = 1;
    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float timeForLostSightEnemy = 10;

    [SerializeField] private Condition whatDo;

    private bool onLostTarget;
    private bool onLostAllied;

    private float timerForLostSightTarget;
    private float lastTimeCheck;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState)
    {
        if(SeeTarget(controller)) return true;
        if(SeeAllied(controller)) return true;

        return false;
    }

    private void Start() => SwichtCondition();

    private void SwichtCondition()
    {
        onLostTarget = false;
        onLostAllied = false;

        switch (whatDo)
        {
            case Condition.OnLostTarget:
                onLostTarget = true;
                break;
            case Condition.OnLostAllied:
                onLostAllied = true;
                break;
        }
    }

    private bool SeeAllied(FSM_Controller controller)
    {
        if (!onLostAllied) return false;

        if (controller.Allied == null)
        {
            controller.CanBeFollowTarget = false; 
            return true;
        }
        else
        {  
            if (controller.Allied.TryGetComponent(out I_Team team))
            {
                if(team.GetAllied() == transform.root)
                {
                    if (controller.CanSeeDebug) Debug.Log("Tu Hai me e Io ho te Non Siamo Compatibili Ti Mollo");

                    controller.Allied = null;
                    controller.CanBeFollowTarget = false;
                    return true;
                }

                if (!team.CanBeFollow())
                {
                    controller.Allied = null;
                    controller.CanBeFollowTarget = false;
                    return true;
                }
            }
            return false;
        }
    }

    private bool SeeTarget(FSM_Controller controller)
    {
        if(!onLostTarget) return false; 
        if(controller.Target != null && !onLostTarget) return true;
        if(controller.Target == null) return true;

        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        Vector3 targetOriginCast = controller.Target.position + new Vector3(0, hight, 0);
        Vector3 direction = targetOriginCast - originCast;

        if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
        {
            if (controller.CanSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 1);

            if (hit.transform == controller.Target) timerForLostSightTarget = 0;
            else timerForLostSightTarget += Time.time - lastTimeCheck;
        }
        else timerForLostSightTarget += Time.time - lastTimeCheck;

        lastTimeCheck = Time.time;

        if (timerForLostSightTarget >= timeForLostSightEnemy)
        {
            timerForLostSightTarget = 0;
            controller.Target = null;
            return true;
        }
        return false;
    }
}
