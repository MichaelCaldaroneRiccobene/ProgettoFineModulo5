using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour, I_Team
{
    public enum StateAI { None, GoOnRandomMove, GoOnTarget }

    [SerializeField] private Transform target;
    [SerializeField] private StateAI stateAI = StateAI.None;

    [SerializeField] private float radiusRandomPosition = 15;
    [SerializeField] private int teamNumber = 2;

    [SerializeField] private float viewAngle = 90;
    [SerializeField] private float hight = 1;

    [SerializeField] private int pointSightDistance = 50;
    [SerializeField] private int sightDistance = 40;

    [SerializeField] private int pointSenseDistance = 50;
    [SerializeField] private int sightSenseDistance = 5;

    [SerializeField] private LayerMask layertTarget;

    private NavMeshAgent agent;
    private NavMeshPath pathToFollw;

    private StateAI curretState;

    private Vector3 pointToGo;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathToFollw = new NavMeshPath();

        curretState = stateAI;

        StartCoroutine(OnSeePotenzialTarget());
    }

    private void Update()
    {
        if (curretState == stateAI) return;

        curretState = stateAI;
        StopAllCoroutines();

        switch (curretState)
        {
            default:
                Debug.LogError("StateAI ERRRRORRRRR");
                break;
            case StateAI.GoOnRandomMove:
                StartCoroutine(GoOnRandomPointRoutin());
                break;
            case StateAI.GoOnTarget:
                StartCoroutine(GoOnTargetRoutin());
                break;
        }
    }

    #region RandomMovement
    private IEnumerator GoOnRandomPointRoutin()
    {
        while (true)
        {
            bool isOnGoRandomPoint = false;

            RandomPoint(agent, agent.transform.position, radiusRandomPosition, out pointToGo);
            while (agent.pathPending) yield return null;

            isOnGoRandomPoint = true;

            while (isOnGoRandomPoint)
            {
                agent.SetDestination(pointToGo);
                if (agent.remainingDistance < 1) isOnGoRandomPoint = false;

                yield return null;
            }
        }
    }

    private void RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range, out Vector3 result)
    {
        Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
        randomPosition.y = agent.transform.position.y;

        if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas)) result = hit.position;
        else result = Vector3.zero;
    }
    #endregion


    #region FollowTarget
    private IEnumerator GoOnTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(0.5f);
        agent.ResetPath();

        while (target != null)
        {
            if (agent.CalculatePath(target.position, pathToFollw)) agent.destination = target.position;

            yield return waitForSeconds;
        }
    }
    #endregion


    #region OnSeePotezialTarget
    private IEnumerator OnSeePotenzialTarget()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        StartCoroutine(OnSenseTargetClose());

        while (true)
        {
            Vector3 originCast = transform.position + new Vector3(0, hight, 0);

            float deltaAngle = (2 * viewAngle / pointSightDistance);

            for(int i = 0; i < pointSightDistance; i++)
            {
                float curretAngle = -viewAngle + deltaAngle * i;
                Vector3 direction = Quaternion.Euler(0,curretAngle, 0) * transform.forward;

                Vector3 point = originCast + direction * sightDistance;

                if(Physics.Raycast(originCast,direction,out RaycastHit hit,sightDistance, layertTarget))
                {
                    if (hit.collider != null)
                    {
                        Debug.Log("Colpito");
                        if (hit.collider.TryGetComponent(out I_Team team))
                        {
                            Debug.Log("Team Number " + team);

                            if (team.GetTeamNumber() == teamNumber)
                            {
                                Debug.Log("On my Team");
                            }
                            else Debug.Log("Enemy Team");
                        }
                    }
                }
            }

            yield return waitForSeconds;
        }
    }

    private IEnumerator OnSenseTargetClose()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(1);
        while (true)
        {
            Vector3 originCast = transform.position + new Vector3(0, hight, 0);

            float deltaAngle = (2 * 360 / pointSenseDistance);

            for (int i = 0; i < pointSenseDistance; i++)
            {
                float curretAngle = -360 + deltaAngle * i;
                Vector3 direction = Quaternion.Euler(0, curretAngle, 0) * transform.forward;

                Vector3 point = originCast + direction * sightSenseDistance;

                if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightSenseDistance, layertTarget))
                {
                    if (hit.collider != null)
                    {
                        Debug.Log("Colpito");
                        if (hit.collider.TryGetComponent(out I_Team team))
                        {
                            Debug.Log("Team Number " + team);

                            if (team.GetTeamNumber() == teamNumber)
                            {
                                Debug.Log("On my Team");
                            }
                            else Debug.Log("Enemy Team");
                        }
                    }
                }
            }

            yield return waitForSeconds;
        }
    }

    public int GetTeamNumber() => teamNumber;

    #endregion
}

