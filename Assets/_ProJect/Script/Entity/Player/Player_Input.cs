using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player_Input : MonoBehaviour, I_Team
{
    [SerializeField] private Stats_EntitySO stats;
    [SerializeField] private int teamNumber = 1;
    [SerializeField] private float rotationSpeed = 50;
    [SerializeField] private Transform head;
    [SerializeField] private float distanceShoot = 15;

    private NavMeshAgent agent;

    private float horizontal;
    private float vertical;
    private Vector3 direction;


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
        if (Physics.Raycast(head.position, transform.forward, out RaycastHit hit, distanceShoot))
        {
            Debug.DrawRay(head.position, hit.point - head.position, Color.black, 1);

            if (hit.collider.TryGetComponent(out LifeSistem lifeSistem))
            {
                lifeSistem.UpdateHp(-stats.DamageMelee);
            }
        }
        else Debug.DrawRay(head.position, transform.forward * distanceShoot, Color.black, 1);
    }

    public void Movement() => agent.velocity = direction.normalized * agent.speed;

    public int GetTeamNumber() => teamNumber;

    public void SetTarget(Transform target) { }

    public void OnDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("I Am Dead");
    }
}
