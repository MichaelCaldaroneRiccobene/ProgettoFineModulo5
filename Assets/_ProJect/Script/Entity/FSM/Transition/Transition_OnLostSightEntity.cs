using UnityEngine;

public class Transition_OnLostSightEntity : AbstractTransition
{
    public enum Condition {OnLostTarget, OnLostAllied }

    [Header("Setting OnLostSightEntity")]
    [SerializeField] private float hight = 1;
    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float timeForLostSightEnemy = 10;

    [SerializeField] private Condition whatToDo;

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

        switch (whatToDo)
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
        // Se non sto cercando per alleato lascio
        if (!onLostAllied) return false;

        // se non ho alleato, non mi possono più seguire e me ne vado
        if (controller.Allied == null)
        {
            controller.CanBeAFollowTarget = false; 
            return true;
        }
        else
        {  
            if (controller.Allied.TryGetComponent(out I_Team team))
            {
                // controllo di sicurezza, se l'alleato che voglio seguire sta seguendo me ,non mi possono più seguire e me ne vado
                if (team.GetAllied() == transform)
                {
                    if (controller.CanSeeDebug) Debug.Log("Tu Hai me e Io ho te Non Siamo Compatibili Ti Mollo");

                    controller.Allied = null;
                    controller.CanBeAFollowTarget = false;
                    return true;
                }

                // se l'alleato che sto seguendo non lo posso più seguire non mi possono più seguire e me ne vado
                if (!team.CanBeFollow())
                {
                    controller.Allied = null;
                    controller.CanBeAFollowTarget = false;
                    return true;
                }
            }
            return false;
        }
    }

    private bool SeeTarget(FSM_Controller controller)
    {
        // se non sto cercando per target lascio
        // se ho il target ma non sto cercando per target me ne vado
        // se non ho il target ma sto cercando per target me ne vado
        if (!onLostTarget) return false; 
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
