using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class EnemyBrain : MonoBehaviour, I_Team
{
    public enum StateAI {None,IsOnStayOnPlaceAndLookAround, IsOnRandomMove,IsOnPatrol,IsOnTarget,IsOnSerchTarget }   
    public enum ComportamentNormalAI {GoStayOnPlace,GoPatrol,GoRandomMove,GoOnTarget }

    [Header("DebugAI")]
    [SerializeField] private StateAI stateAI = StateAI.None;
    [SerializeField] private Transform target;
    [SerializeField] private bool canSeeDebug;

    [Header("SettingForAI")]
    [SerializeField] private ComportamentNormalAI normalState;

    [SerializeField] private float timeUpdateRoutine = 1;
    [SerializeField] private float timeUpdateSightRoutine = 0.4f;
    [SerializeField] private float stopDistanceToDestination = 2;

    [SerializeField] private int teamNumber = 2;

    [Header("SettingSenseSight")]
    [SerializeField] private float viewAngleForward = 90;
    [SerializeField] private float viewAngleBack = 90;
    [SerializeField] private float hight = 1;

    [SerializeField] private int sightDistance = 12;
    [SerializeField] private int sightSenseDistance = 5;
    [SerializeField] private int callTeamMateDistance = 25;

    [Header("SettingRayToAddForSight")]
    [SerializeField] private int raySightToAdd = 50;   
    [SerializeField] private int raySenseToAdd = 50;

    [Header("SettingForStayOnPlaceAndLookAround")]
    [SerializeField] private float timeForStayOnPlaceAndLookAround = 5;

    [Header("SettingForPatrol")]
    [SerializeField] private Transform[] pointsForPatrol;

    [Header("SettingForRandomMove")]
    [SerializeField] private float radiusRandomPosition = 15;

    [Header("SettingForFollowTarget")]
    [SerializeField] private float radiusSerchPosition = 5;

    [Header("SettingForSerchTarget")]
    [SerializeField] private float timeForLostSightEnemy = 10;

    private StateAI currentState;

    private NavMeshAgent agent;
    private NavMeshPath pathToFollw;

    private Vector3 pointToGo;

    private Vector3 startPosition;
    private Quaternion startRotation;

    private float timerForLostSightTarget;

    private bool iSeeTarget;

    private Transform lastSeeTarget;

    private Coroutine currentCoroutine;

    #region StartAndUpdate
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        pathToFollw = new NavMeshPath();

        startPosition = transform.position;
        startRotation = agent.transform.rotation;
        SetUpCorotine();
    }

    private void Update()
    {
        LookOnTarget();


        ChangeStateAI();
    }
    #endregion

    #region OnStayInPlaceAndLoockAround
    private IEnumerator GoOnStayInPlaceAndLoockAroundRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeForStayOnPlaceAndLookAround);
        while (!iSeeTarget)
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
        yield break;
    }

    private IEnumerator GoOnStartPosition()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        agent.SetDestination(startPosition);
        while (agent.pathPending) yield return null;

        bool isOnStartPosition = false;
        while (!isOnStartPosition)
        {
            if (agent.remainingDistance < stopDistanceToDestination) isOnStartPosition = true;

            yield return waitForSeconds;
        }

        bool isOnStartRotation = false;
        while (!isOnStartRotation)
        {
            agent.updateRotation = false;
            Quaternion curretRotation = agent.transform.rotation;

            float velocityRotation = 10;
            float progress = 0;

            while(progress < 1)
            {
                progress += Time.deltaTime * velocityRotation;
                agent.transform.rotation = Quaternion.Lerp(curretRotation,startRotation, progress);
                yield return null;
            }
            isOnStartRotation = true;
            agent.updateRotation = true;
        }

        if(target == null) StartCoroutine(GoOnStayInPlaceAndLoockAroundRoutine());
    }
    #endregion

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
        int numberOfTentativ = 100;

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
    #endregion

    #region Patrolling
    private IEnumerator GoOnPatrolRoutine()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateRoutine);

        if (pointsForPatrol.Length <= 0) stateAI = StateAI.IsOnRandomMove;
        int destinationForPatrolIndex = 0;

        while (true)
        {
            bool isOnGoOnPatrol = false;

            if (agent.CalculatePath(pointsForPatrol[destinationForPatrolIndex].position, pathToFollw)) pointToGo = pointsForPatrol[destinationForPatrolIndex].position;
            while (agent.pathPending) yield return null;

            isOnGoOnPatrol = true;
            agent.SetDestination(pointToGo);

            while (isOnGoOnPatrol)
            {
                if (agent.remainingDistance < stopDistanceToDestination) isOnGoOnPatrol = false;

                yield return waitForSeconds;
            }
            destinationForPatrolIndex = (destinationForPatrolIndex + 1) % pointsForPatrol.Length;
        }
    }
    #endregion

    #region FollowTarget
    private IEnumerator GoOnTargetRoutin()
    {
        if (target != null)
        {
            WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);

            timerForLostSightTarget = timeForLostSightEnemy;
            agent.updateRotation = !iSeeTarget;
            agent.ResetPath();

            while (target != null)
            {
                SeeTarget();
                if (agent.CalculatePath(target.position, pathToFollw))
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);

                    if (distanceToTarget > stopDistanceToDestination && !isAttackMelee) agent.destination = target.position;
                    else
                    {
                        StartCoroutine(OnAttackMelee());
                        agent.ResetPath();
                    }
                }
                yield return waitForSeconds;
            }
        }
        else SelectStateNormal();

    }

    // Temporaneo

    public UnityEvent OnAttack;
    private bool isAttackMelee = false;
    public IEnumerator OnAttackMelee()
    {
        if(isAttackMelee) yield break;
        isAttackMelee = true;
        yield return new WaitForSeconds(1f);
        OnAttack?.Invoke();
        yield return new WaitForSeconds(0.5f);
        isAttackMelee = false;
    }

    private void LookOnTarget() { if (target != null && iSeeTarget) agent.transform.LookAt(target.position); }

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
                iSeeTarget = true;
                timerForLostSightTarget = timeForLostSightEnemy;              
            }
            else
            {
                TimerLostSightTarget();
                if(target != null)
                {
                    float distanceToTarget = Vector3.Distance(transform.position, target.position);
                    if (distanceToTarget <= stopDistanceToDestination) iSeeTarget = true;
                    else iSeeTarget = false;
                }
            }
        }
    }

    private void TimerLostSightTarget()
    {
        timerForLostSightTarget -= timeUpdateRoutine;
        if (canSeeDebug) Debug.Log("Timer For Lost Sight Target " + timerForLostSightTarget);

        if (timerForLostSightTarget <= 0)
        {
            agent.updateRotation = true;
            if (canSeeDebug) Debug.Log("I Lost Target");
            stateAI = StateAI.IsOnSerchTarget;
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
            SelectStateNormal();
        }
        else
        {
            target = null;
            SelectStateNormal();
        }
    }
    #endregion  

    #region OnSeePotezialTarget
    private IEnumerator OnSeePotenzialTargetRoutin()
    {
        WaitForSeconds waitForSeconds = new WaitForSeconds(timeUpdateSightRoutine);
        int angleToCover = 180;

        while (true)
        {
            if(!target)
            {
                OnSeeOrSenseTarget(raySightToAdd, sightDistance, viewAngleForward, transform.forward, Color.yellow);
                OnSeeOrSenseTarget(raySenseToAdd, sightSenseDistance, viewAngleBack, -transform.forward, Color.green);
            }
            else OnSeeOrSenseTarget(raySightToAdd, callTeamMateDistance, angleToCover, transform.forward, Color.cyan);

            yield return waitForSeconds;
        }
    }

    #endregion  

    #region Utility

    private void ChangeStateAI()
    {
        if (currentState == stateAI) return;

        currentState = stateAI;
        SetNullCurrentCorotine();

        switch (currentState)
        {
            // Errore e Va In GoRandomMove
            default:
                Debug.LogError("StateAI ERRRRORRRRR", transform);
                stateAI = StateAI.IsOnRandomMove;
                break;

            // Non fa Niente
            case StateAI.None:
                break;

            // Si guarda attorno da ferma
            case StateAI.IsOnStayOnPlaceAndLookAround:
                currentCoroutine = StartCoroutine(GoOnStartPosition());
                break;

            // Gira per il mondo libera e felice 
            case StateAI.IsOnRandomMove:
                currentCoroutine = StartCoroutine(GoOnRandomPointRoutin());
                break;

            // Un soldato attento che fa il suo percorsino
            case StateAI.IsOnPatrol:
                currentCoroutine = StartCoroutine(GoOnPatrolRoutine());
                break;

            // Ti insegue come se fosse l'unica cosa che deve fare 
            case StateAI.IsOnTarget:
                currentCoroutine = StartCoroutine(GoOnTargetRoutin());
                break;

            // Ti vuole cercare 
            case StateAI.IsOnSerchTarget:               
                currentCoroutine = StartCoroutine(GoOnSerchTargetRoutin());
                break;
        }
    }

    private void SelectStateNormal()
    {
        switch (normalState)
        {
            case ComportamentNormalAI.GoStayOnPlace:
                stateAI = StateAI.IsOnStayOnPlaceAndLookAround;
                break;
            case ComportamentNormalAI.GoPatrol:
                stateAI = StateAI.IsOnPatrol;
                break;
            case ComportamentNormalAI.GoRandomMove:
                stateAI = StateAI.IsOnRandomMove;
                break;
        }
    }

    private void SetUpCorotine() => StartCoroutine(OnSeePotenzialTargetRoutin());

    private void SetNullCurrentCorotine()
    {
        if (currentCoroutine == null) return;

        StopCoroutine(currentCoroutine);
        currentCoroutine = null;
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
                            if(target != null) team.SetTarget(target);
                        }
                        else
                        {
                            lastSeeTarget = hit.transform;
                            target = lastSeeTarget;
                            stateAI = StateAI.IsOnTarget;
                        }
                    }
                }
            }
            else { if (canSeeDebug) Debug.DrawRay(originCast, direction * sightDistance, color, 0.1f); }
        }
    }

    public int GetTeamNumber() => teamNumber;

    public void SetTarget(Transform target)
    {
        Debug.Log("colpito");
        if(target.gameObject.TryGetComponent(out I_Team team))
        {
            Debug.Log("Ha Team");

            if (team.GetTeamNumber() != teamNumber)
            {
                Debug.Log("No Mio Team");
                if (lastSeeTarget == null)
                {
                    Debug.Log("NSetto target");
                    lastSeeTarget = target;
                    this.target = lastSeeTarget;
                    stateAI = StateAI.IsOnTarget;
                }
            }
        }
        else Debug.Log("NO Team");
    }

    public void OnDead() => Destroy(gameObject);
    #endregion
}

