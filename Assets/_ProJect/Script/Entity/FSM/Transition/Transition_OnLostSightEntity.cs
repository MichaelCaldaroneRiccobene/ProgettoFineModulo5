using UnityEngine;

public class Transition_OnLostSightEntity : AbstractTransition
{
    public enum WhatToDo 
    { 
        None = 0, OnLostTarget = 1, OnLostAllied = 2
    }

    [Header("Setting OnLostSightEntity")]
    [SerializeField] private float hight = 1;
    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float timeForLostSightEnemy = 10;

    [SerializeField] private WhatToDo whatToDo;

    private bool onLostTarget;
    private bool onLostAllied;

    private float timerForLostSightTarget;
    private float lastTimeCheck;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState)
    {
        if (SeeTarget(controller)) return true;
        if (SeeAllied(controller)) return true;

        return false;
    }

    private void Start() => SelectWhatToDo();

    private void SelectWhatToDo()
    {
        onLostTarget = false;
        onLostAllied = false;

        switch (whatToDo)
        {
            case WhatToDo.OnLostTarget:
                onLostTarget = true;
                break;
            case WhatToDo.OnLostAllied:
                onLostAllied = true;
                break;
        }
    }

    private bool SeeAllied(FSM_Controller controller)
    {
        // Se non sto cercando per alleato lascio
        if (!onLostAllied) return false;

        // se non ho alleato, non mi possono più seguire e me ne vado
        if (controller.GetAllied() == null)
        {
            controller.CanBeAFollowTarget = false;
            return true;
        }
        else
        {
            // se alleato muore vado via
            if (controller.GetAllied().TryGetComponent(out LifeSistem lifeSistem))
            {
                if (lifeSistem.IsDead())
                {
                    OnLostAllied(controller);
                    return true;
                }
            }


            if (controller.GetAllied().TryGetComponent(out I_Team team))
            {
                // controllo di sicurezza, se l'alleato che voglio seguire sta seguendo me ,non mi possono più seguire e me ne vado
                if (team.GetAllied() == transform)
                {
                    if (controller.CanSeeDebug) Debug.Log("Tu Hai me e Io ho te Non Siamo Compatibili Ti Mollo");

                    OnLostAllied(controller);
                    return true;
                }

                // se l'alleato che sto seguendo non lo posso più seguire non mi possono più seguire e me ne vado
                if (!team.CanBeFollow())
                {
                    OnLostAllied(controller);
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
        if (controller.GetTarget() != null && !onLostTarget) return true;
        if (controller.GetTarget() == null) return true;

        // se target muore vado via
        if (controller.GetTarget().TryGetComponent(out LifeSistem lifeSistem))
        {
            if (lifeSistem.IsDead())
            {
                OnLostTarget(controller);
                return true;
            }
        }

        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        Vector3 targetOriginCast = controller.GetTarget().position + new Vector3(0, hight, 0);
        Vector3 direction = targetOriginCast - originCast;

        if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
        {
            if (controller.CanSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 1);

            if (hit.transform == controller.GetTarget()) timerForLostSightTarget = 0;
            else timerForLostSightTarget += Time.time - lastTimeCheck;
        }
        else timerForLostSightTarget += Time.time - lastTimeCheck;

        lastTimeCheck = Time.time;

        if (timerForLostSightTarget >= timeForLostSightEnemy)
        {
            OnLostTarget(controller);
            return true;
        }
        return false;
    }
    private void OnLostAllied(FSM_Controller controller)
    {
        controller.ClearAllied();
        controller.CanBeAFollowTarget = false;
    }

    private void OnLostTarget(FSM_Controller controller)
    {
        timerForLostSightTarget = 0;
        controller.ClearTarget();
    }
}
