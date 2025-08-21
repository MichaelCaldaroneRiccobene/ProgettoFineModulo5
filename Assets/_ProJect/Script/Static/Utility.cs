using UnityEngine;
using UnityEngine.AI;

public static class Utility
{
    public static bool OnSeeOrSenseTarget(FSM_Controller controller, float hight, int rayToAdd, float sightDistance, float viewAngle, Vector3 forward, bool isCheckForTarget, bool isCheckForAllied, bool isFollowAllie, Color color)
    {
        Vector3 originCast = controller.transform.position + new Vector3(0, hight, 0);
        float deltaAngle = (2 * viewAngle) / (rayToAdd - 1);

        // Se cerco Allied e ho Allied Io Felice ;)
        if (isFollowAllie)
        {
            if (controller.Allied != null) return true;
        }

        // Se cerco Target e ho target o mi hanno dato target Io Felice ;)
        if (isCheckForTarget)
        {
            if (controller.Target != null)
            {
                controller.LastTarget = controller.Target;
                return true;
            }
        }

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
                        // Vede Amici :)
                        if (hitEntity.GetTeamNumber() == controller.TeamNumber)
                        {
                            if (isCheckForAllied)
                            {
                                // Se vedo Amico Sono Felice e li Do il Target :D
                                if (controller.Target != null)
                                {
                                    if (!hitEntity.HasTarget()) hitEntity.SetTarget(controller.Target);
                                }
                            }

                            if (isFollowAllie)
                            {
                                // Se vedo Amico Sono Felice e li Do il Target ;D
                                if (controller.Target != null)
                                {
                                    if (!hitEntity.HasTarget()) hitEntity.SetTarget(controller.Target);
                                }

                                if(hitEntity.CanBeFollow())
                                {
                                    controller.Allied = hit.transform;
                                    return true;
                                }
                            }
                        }

                        // Vede Nemici :(
                        if (hitEntity.GetTeamNumber() != controller.TeamNumber)
                        {
                            if (isCheckForTarget)
                            {
                                // Se vedo Target Sono Felice
                                controller.LastTarget = hit.transform;
                                controller.Target = controller.LastTarget;

                                return true;
                            }
                        }
                    }
                }
            }
            else if (controller.CanSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f);
        }
        return false;
    }


    public static void RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range, out Vector3 result)
    {
        int numberOfTentativ = 5;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
            randomPosition.y = agent.transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return;
            }
        }
        result = Vector3.zero;
    }

    public static Vector3 RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range)
    {
        int numberOfTentativ = 5;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
            randomPosition.y = agent.transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas)) return hit.position;
        }
        return Vector3.zero;
    }

    public static void RandomPoint(NavMeshAgent agent, Vector3 startPosition, float minRange, float maxRange, out Vector3 result)
    {
        int numberOfTentativ = 5;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            Vector3 randomPosition = Random.insideUnitSphere * Random.Range(minRange, maxRange) + startPosition;
            randomPosition.y = agent.transform.position.y;

            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return;
            }
        }
        result = Vector3.zero;
    }
}
