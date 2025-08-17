using UnityEngine;

public class Transition_OnSeeTarget : AbstractTransition
{
    [SerializeField] private float viewAngleForward = 60;
    [SerializeField] private float viewAngleBack = 120;

    [SerializeField] private float sightDistance = 12;
    [SerializeField] private float sightSenseDistance = 4;

    [SerializeField] private int raySightToAdd = 70;
    [SerializeField] private int raySenseToAdd = 30;

    [SerializeField] private float hight = 1;
    [SerializeField] private bool canSeeDebug;

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) 
    {
        if(OnSeeOrSenseTarget(controller,raySightToAdd, sightDistance, viewAngleForward, transform.forward, Color.yellow)) return true;
        
        if(OnSeeOrSenseTarget(controller,raySenseToAdd, sightSenseDistance, viewAngleBack, -transform.forward, Color.green)) return true;

        return false;
    }

    private bool OnSeeOrSenseTarget(FSM_Controller controller,int rayToAdd, float sightDistance, float viewAngle, Vector3 forward, Color color)
    {
        if(controller.Target != null)
        {
            controller.LastTarget = controller.Target;
            return true;
        }

        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        float deltaAngle = (2 * viewAngle) / (rayToAdd - 1);

        for (int i = 0; i < rayToAdd; i++)
        {
            float curretAngle = -viewAngle + deltaAngle * i;
            Vector3 direction = Quaternion.Euler(0, curretAngle, 0) * forward;

            if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
            {
                if (canSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 0.1f);

                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out I_Team hitEntity))
                    {
                        if (hitEntity.GetTeamNumber() != controller.TeamNumber)
                        {
                            // Se vedo Target Sono Felice

                            controller.LastTarget = hit.transform;
                            controller.Target = hit.transform;
                            return true;
                        }
                    }
                }
            }
            else if (canSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f);
        }
        return false;
    }
}
