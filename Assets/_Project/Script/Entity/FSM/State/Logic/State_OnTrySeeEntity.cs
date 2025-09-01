using System.Collections;
using UnityEngine;

public class State_OnTrySeeEntity : AbstractState
{
    [Header("Setting RandomMove")]
    [SerializeField] private float timeUpdateRoutine = 0.2f;

    [SerializeField] private bool shareTarget;
    [Header("Setting For See")]
    [SerializeField] private float viewAngleForward = 60;
    [SerializeField] private float viewAngleBack = 120;

    [SerializeField] private float angleShareTarget = 360;

    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float sightSenseDistance = 4;

    [SerializeField] private int raySightToAdd = 70;
    [SerializeField] private int raySenseToAdd = 30;

    [SerializeField] private int rayShareTarget = 100;

    [SerializeField] private float hight = 1;

    public override void StateEnter() 
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State OnTrySeeEntity");

        StartCoroutine(TrySeeCoroutine());
    }

    public override void StateUpdate() { }

    public override void StateExit() 
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State OnTrySeeEntity");

        StopAllCoroutines();
    }

    private IEnumerator TrySeeCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        while (true)
        {
            TrySee(hight, raySightToAdd, sightDistance, viewAngleForward,transform.forward, Color.yellow);
            TrySee(hight, raySenseToAdd, sightSenseDistance, viewAngleBack, -transform.forward, Color.green);

            OnShareTarget();
            yield return waitForSeconds;
        }
    }

    private void TrySee(float hight, int rayToAdd, float sightDistance, float viewAngle, Vector3 forward, Color color)
    {
        if (shareTarget) return;

        Vector3 originCast = controller.transform.position + new Vector3(0, hight, 0);
        float deltaAngle = (2 * viewAngle) / (rayToAdd - 1);

        for (int i = 0; i < rayToAdd; i++)
        {
            float curretAngle = -viewAngle + deltaAngle * i;
            Vector3 direction = Quaternion.Euler(0, curretAngle, 0) * forward;

            if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
            {
                if (controller.CanSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 0.1f);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out I_Team hitEntity))
                    {
                        if (hit.collider.TryGetComponent(out LifeSistem lifeSistem) && lifeSistem.IsDead()) continue;

                        // Vede Amici :)
                        if (hitEntity.GetTeamNumber() == controller.GetTeamNumber()) SetAllied(hitEntity, hit.collider.transform);
                        else SetTarget(hit.collider.transform); // Vede Nemici :(
                    }
                }
            }
            else if (controller.CanSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f);
        }
    }

    private void OnShareTarget()
    {
        if (!shareTarget) return;

        Vector3 originCast = controller.transform.position + new Vector3(0, hight, 0);
        float deltaAngle = (2 * angleShareTarget) / (rayShareTarget - 1);

        for (int i = 0; i < rayShareTarget; i++)
        {
            float curretAngle = -rayShareTarget + deltaAngle * i;
            Vector3 direction = Quaternion.Euler(0, curretAngle, 0) * transform.forward;

            if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
            {
                if (controller.CanSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 0.1f);

                if (hit.collider.TryGetComponent(out I_Team hitEntity)) ShareTarget(hitEntity);
            }
            else if (controller.CanSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, Color.red, 0.1f);
        }
    }

    private void ShareTarget(I_Team hitEntity)
    {
        if (controller.GetTarget() != null) { if (!hitEntity.HasTarget()) hitEntity.SetTarget(controller.GetTarget()); }
    }

    private void SetAllied(I_Team hitEntity,Transform hit)
    {
        if(controller.GetAllied() != null) return;

        if (hitEntity.CanBeFollow()) controller.SetAllied(hit.transform);
    }

    private void SetTarget(Transform hit)
    {
        if (controller.GetTarget() != null) return;

        controller.SetPriorityTarget(hit.transform);
        controller.SetLastTarget(controller.GetTarget());
    }
}
