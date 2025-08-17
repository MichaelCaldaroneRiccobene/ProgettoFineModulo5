using System;
using System.Collections;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;

public class State_FollowTarget : AbstractState
{
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;
    [SerializeField] private float stopDistanceToDestination = 2f;

    [SerializeField] private float hight = 1;
    [SerializeField] private bool canSeeDebug;

    public event Action OnTryMeleeAttack;

    private NavMeshAgent agent;
    private NavMeshPath pathToFollw;

    public override void StateEnter()
    {
        Debug.Log("Entrato in State FollowTarget");

        if (agent == null) agent = GetComponentInParent<NavMeshAgent>();

        pathToFollw = new NavMeshPath();
        agent.ResetPath();
        agent.updateRotation = false;
        StartCoroutine(GoOnTargetRoutin());
    }

    public override void StateExit()
    {
        Debug.Log("Uscito dallo State FollowTarget");

        StopAllCoroutines();
        agent.ResetPath();
        agent.updateRotation = true;
    }

    public override void StateUpdate() { LookOnTarget(); }

    private IEnumerator GoOnTargetRoutin()
    {
        if (controller.Target != null)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
            agent.ResetPath();

            while (controller.Target != null)
            {
                if (agent.CalculatePath(controller.Target.position, pathToFollw))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, controller.Target.position);

                    if (distanceToTarget > stopDistanceToDestination) agent.destination = controller.Target.position;
                    else
                    {
                        OnTryMeleeAttack?.Invoke();
                        agent.ResetPath();
                    }
                }

                OnSeeOrSenseTarget(controller,100,12,180,transform.forward,Color.blue);
                yield return waitForSeconds;
            }
        }
    }

    private void OnSeeOrSenseTarget(FSM_Controller controller, int rayToAdd, float sightDistance, float viewAngle, Vector3 forward, Color color)
    {
        if (controller.Target == null) return;

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
                        if (hitEntity.GetTeamNumber() == controller.TeamNumber)
                        {
                            // Se vedo Amico Sono Felice e li Do il Target

                            hitEntity.SetTarget(controller.Target);
                            return ;
                        }
                    }
                }
            }
            else if (canSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f);
        }
        return;
    }

    private void LookOnTarget() { if (controller.Target != null) agent.transform.LookAt(controller.Target.position); }
}
