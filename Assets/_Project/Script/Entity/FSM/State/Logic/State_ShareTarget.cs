using System.Collections;
using UnityEngine;

public class State_ShareTarget : AbstractState
{
    [SerializeField] private float timeUpdateRoutine = 0.2f;

    [Header("Setting For Shaare")]
    [SerializeField] private float angleShareTarget = 360;

    [SerializeField] private float sightDistance = 12;
    [SerializeField] private int rayShareTarget = 100;

    [SerializeField] private float hight = 1;

    public override void StateEnter()
    {
        if (controller.CanSeeDebug) Debug.Log("Entrato in State AbstractState");

        StartCoroutine(TrySeeCoroutine());
    }

    public override void StateUpdate() { }

    public override void StateExit()
    {
        if (controller.CanSeeDebug) Debug.Log("Uscito dallo State AbstractState");

        StopAllCoroutines();
    }

    private IEnumerator TrySeeCoroutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        while (true)
        {
            OnShareTarget();
            yield return waitForSeconds;
        }
    }
    private void OnShareTarget()
    {
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
}
