using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyBrain : MonoBehaviour, I_Team
{
    public enum StateAI { None, GoOnRandomMove, GoOnTarget, GoOnSerchTarget, GoOnStayOnPlaceAndLookAround }   
    [SerializeField] private StateAI stateAI = StateAI.None;
    [SerializeField] private bool canSeeDebug;
    [SerializeField] private float timeUpdateRoutine = 1;
    [SerializeField] private float timeUpdateSightRoutine = 0.4f;

    public Transform targetToLook;

    [SerializeField] private float radiusRandomPosition = 15;
    [SerializeField] private float radiusSerchPosition = 5;
    [SerializeField] private int teamNumber = 2;

    [SerializeField] private float timeForLostSightEnemy = 10;
    [SerializeField] private float viewAngleForward = 90;
    [SerializeField] private float viewAngleBack = 90;
    [SerializeField] private float hight = 1;

    [SerializeField] private int raySightToAdd = 50;
    [SerializeField] private int sightDistance = 40;
    [SerializeField] private float stopDistanceToDestination = 2;

    [SerializeField] private int raySenseToAdd = 50;
    [SerializeField] private int sightSenseDistance = 5;

    private NavMeshAgent agent;
    private NavMeshPath pathToFollw;

    private StateAI curretState;

    private Vector3 pointToGo;

    private float timerForLostSightEnemy;

    public Transform target;
    private Transform lastSeeTarget;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathToFollw = new NavMeshPath();
    }

    private void Update()
    {
        if(canSeeDebug) Debug.Log(stateAI,transform);
        if (curretState == stateAI) return;

        curretState = stateAI;
        StopAllCoroutines();

        switch (curretState)
        {
            default:
                Debug.LogError("StateAI ERRRRORRRRR", transform);
                stateAI = StateAI.GoOnRandomMove;
                break;
            case StateAI.None:
                break;
            case StateAI.GoOnRandomMove:
                StartCoroutine(OnSeePotenzialTargetRoutin());
                StartCoroutine(GoOnRandomPointRoutin());
                break;
            case StateAI.GoOnTarget:
                StartCoroutine(GoOnTargetRoutin());
                break;
            case StateAI.GoOnSerchTarget:
                StartCoroutine(OnSeePotenzialTargetRoutin());
                StartCoroutine(GoOnSerchTargetRoutin());
                break;
            case StateAI.GoOnStayOnPlaceAndLookAround:
                StartCoroutine(OnSeePotenzialTargetRoutin());
                StartCoroutine(GoOnStayInPlaceAndLoockAroundRoutine()); 
                break;
        }
    }

    #region RandomMovement
    private IEnumerator GoOnRandomPointRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        while (true)
        {
            bool isOnGoRandomPoint = false;

            RandomPoint(agent, agent.transform.position, radiusRandomPosition, out pointToGo);
            while (agent.pathPending) yield return null;

            isOnGoRandomPoint = true;
            agent.SetDestination(pointToGo);

            while (isOnGoRandomPoint)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoRandomPoint = false;

                yield return waitForSeconds;
            }
        }
    }

    private void RandomPoint(NavMeshAgent agent, Vector3 startPosition, float range, out Vector3 result)
    {
        Vector3 randomPosition = Random.insideUnitSphere * range + startPosition;
        randomPosition.y = agent.transform.position.y;

        int numberOfTentativ = 100;

        for (int i = 0; i < numberOfTentativ; i++)
        {
            if (NavMesh.SamplePosition(randomPosition, out NavMeshHit hit, 1, NavMesh.AllAreas))
            {
                result = hit.position;
                return;
            }
        }        
        result = Vector3.zero;
    }
    #endregion

    #region FollowTarget
    private IEnumerator GoOnTargetRoutin()
    {
        if (target != null)
        {
            StartCoroutine(ComunicatePositionTargetRoutin());
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);

            timerForLostSightEnemy = timeForLostSightEnemy;
            agent.ResetPath();

            while (target != null)
            {
                SeeTarget();
                if (agent.CalculatePath(target.position, pathToFollw))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (distanceToTarget > stopDistanceToDestination) agent.destination = target.position;
                    else agent.ResetPath();
                }
                yield return waitForSeconds;
            }
        }
        else stateAI = StateAI.GoOnRandomMove;

    }

    private void SeeTarget()
    {
        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        Vector3 targetOriginCast = target.position + new Vector3(0, hight, 0);
        Vector3 direction = targetOriginCast - originCast;

        if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
        {
            if (hit.transform == lastSeeTarget)
            {
                if (canSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 1);
                timerForLostSightEnemy = timeForLostSightEnemy;              
            }
            else TimerLostSightTarget();
        }
        else TimerLostSightTarget();
    }

    private void TimerLostSightTarget()
    {
        timerForLostSightEnemy--;

        if (timerForLostSightEnemy <= 0)
        {
            Debug.Log("I Lost Target");
            stateAI = StateAI.GoOnSerchTarget;
        }
    }
    #endregion

    #region OnSeePotezialTarget
    private IEnumerator OnSeePotenzialTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        while (true)
        {
            OnSeeOrSenseTarget(raySightToAdd, sightDistance, viewAngleForward, transform.forward, Color.yellow);
            OnSeeOrSenseTarget(raySenseToAdd, sightSenseDistance, viewAngleBack, -transform.forward, Color.green);

            yield return waitForSeconds;
        }
    }

    #endregion

    #region OnSerchTarget
    private IEnumerator GoOnSerchTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        if (target != null)
        {
            bool isOnGoPoint = false;

            RandomPoint(agent, target.position, radiusSerchPosition, out pointToGo);
            while (agent.pathPending) yield return null;

            agent.SetDestination(pointToGo);
            while (!isOnGoPoint)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoPoint = true;

                yield return waitForSeconds;
            }

            target = null;
            stateAI = StateAI.GoOnRandomMove;
        }
        else stateAI = StateAI.GoOnRandomMove;
    }
    #endregion

    #region OnStayInPlaceAndLoockAround
    private IEnumerator GoOnStayInPlaceAndLoockAroundRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(2.5f);
        while(true)
        {
            Quaternion startRotation = agent.transform.rotation;
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward * sightDistance * -1);

            float progress = 0;

            while (progress < 1)
            {
                progress += Time.deltaTime;
                agent.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);

                yield return null;
            }

            yield return waitForSeconds;
        }       
    }
    #endregion

    #region Utility

    private IEnumerator ComunicatePositionTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(2.5f);

        if (target != null)
        {
            while(true)
            {
                OnSeeOrSenseTarget(raySightToAdd, sightDistance, 180, transform.forward, Color.cyan);
                yield return waitForSeconds;
            }
        }
    }

    private void OnSeeOrSenseTarget(int rayToAdd, float sightDistance, float viewAngle, Vector3 forward, Color color)
    {
        Vector3 originCast = transform.position + new Vector3(0, hight, 0);
        float deltaAngle = (2 * viewAngle) / (rayToAdd - 1);

        for (int i = 0; i < rayToAdd; i++)
        {
            float curretAngle = -viewAngle + deltaAngle * i;
            Vector3 direction = Quaternion.Euler(0, curretAngle, 0) * forward;

            if (Physics.Raycast(originCast, direction, out RaycastHit hit, sightDistance))
            {
                if(canSeeDebug) Debug.DrawLine(originCast, hit.point, Color.red, 0.1f);
                if (hit.collider != null)
                {
                    if (hit.collider.TryGetComponent(out I_Team team))
                    {
                        if (team.GetTeamNumber() == teamNumber)
                        {
                            if(target != null) team.SetTArget(target);
                        }
                        else
                        {
                            lastSeeTarget = hit.transform;
                            target = lastSeeTarget;
                            stateAI = StateAI.GoOnTarget;
                        }
                    }
                }
            }
            else { if (canSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f); }
        }
    }

    public int GetTeamNumber() => teamNumber;

    public void SetTArget(Transform target)
    {
        this.target = target;

        if(lastSeeTarget ==  null)
        {
            lastSeeTarget = this.target;
            stateAI = StateAI.GoOnTarget;
        }
    }
    #endregion

}

