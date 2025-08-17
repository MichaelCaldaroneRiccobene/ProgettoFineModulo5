using System.Collections;
using UnityEngine;

public class Transition_OnLostSightTarget : AbstractTransition
{
    [SerializeField] private float hight = 1;
    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float timeForLostSightEnemy = 10;

    [SerializeField] private bool canSeeDebug;

    private float timerForLostSightTarget;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) => !SeeTarget(controller);

    private bool SeeTarget(FSM_Controller controller)
    {
        if(controller.Target == null) return false;

        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        Vector3 targetOriginCast = controller.Target.position + new Vector3(0, hight, 0);
        Vector3 direction = targetOriginCast - originCast;

        if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
        {
            if (canSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 1);

            if (hit.transform == controller.Target) timerForLostSightTarget = 0;
            else timerForLostSightTarget += Time.deltaTime;
        }
        else
        {
            if (canSeeDebug) Debug.DrawLine(originCast, direction.normalized * sightDistance, Color.red, 1);

            timerForLostSightTarget += Time.deltaTime;
        }

        if (timerForLostSightTarget >= timeForLostSightEnemy)
        {
            controller.Target = null;
            return false;
        }
        return true;
    }
}
