using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Player_Input : MonoBehaviour, I_Team
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private int teamNumber = 1;
    [SerializeField] private float rotationSpeed = 50;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceShoot = 15;

    [SerializeField] private FireBall fireBall;
    [SerializeField] private Transform pointFireBall;

    private NavMeshAgent agent;

    private float horizontal;
    private float vertical;
    private Vector3 direction;

    private bool isDashing;


    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false;
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        if(Input.GetKeyDown(KeyCode.Q))  RegenerateNavMesh.Instance.UpdateNaveMeshSurface();

        if (Input.GetKeyDown(KeyCode.E)) Interction();

        if (Input.GetMouseButtonDown(0)) OnRangeAttack();

        if (Input.GetKeyDown(KeyCode.Alpha1)) Time.timeScale = 1;
        if (Input.GetKeyDown(KeyCode.Alpha2)) Time.timeScale = 0;

        if (Input.GetKeyDown(KeyCode.Space)) StartCoroutine(Dash());
    }

    private void FixedUpdate()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if(Physics.Raycast(ray, out RaycastHit hit))
        {
            Vector3 lookDirection = hit.point - transform.position;
            lookDirection.y = 0f;

            Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 3.5f);
        }


        direction = transform.forward * vertical + transform.right * horizontal;

        Movement();
    }

    private void Interction()
    {
        if(Physics.Raycast(head.position,transform.forward, out RaycastHit hit, 2))
        {
            Debug.Log(hit.collider.name);
            Debug.DrawRay(head.position, hit.point,Color.black,1);

            if(hit.collider.TryGetComponent(out I_Interection interection))
            {
                interection.Interact();
            }
        }
        else Debug.DrawRay(head.position, transform.forward, Color.black, 1);
    }

    private void OnRangeAttack()
    {
        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.FireBallObjForPool);

        if (obj.gameObject.TryGetComponent(out FireBall fireball))
        {
            float shooterSpeed = 0;
            if (agent.velocity.magnitude > 0 && vertical > 0) shooterSpeed = agent.velocity.magnitude;


            fireball.transform.position = pointFireBall.position;
            fireball.OnShoot(transform.forward, shooterSpeed, stats.DamageRange, transform);
            CameraShake.Instance.OnCameraShake(0.5f, 0.5f);
        }
    }

    public void Movement() => agent.velocity = direction.normalized * agent.speed;

    public IEnumerator Dash()
    {
        if(isDashing)yield break;

        isDashing = true;
        Vector3 currentPosition = agent.transform.position;
        Vector3 newPosition = currentPosition + direction.normalized * 5;
        agent.updatePosition = false;

        RaycastHit hit;
        if(Physics.Raycast(transform.position, direction.normalized, out hit, 5))
        {
            Debug.Log(hit.collider.name);
            newPosition = hit.point - direction.normalized * 1f;
        }

        float distanceDash = Vector3.Distance(currentPosition, newPosition);
        float progress = 0;

        while (progress < 1)
        {
            progress += Time.deltaTime * 20/ distanceDash;
            agent.transform.position = Vector3.Lerp(currentPosition, newPosition, progress);

            yield return null;
        }

        agent.updatePosition = true;
        agent.Warp(newPosition);
        isDashing = false;
        Debug.DrawRay(transform.position, newPosition, Color.red, 1);
    }

    public int GetTeamNumber() => teamNumber;

    public void SetTarget(Transform target) { }

    public void OnDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("I Am Dead");
    }
}
