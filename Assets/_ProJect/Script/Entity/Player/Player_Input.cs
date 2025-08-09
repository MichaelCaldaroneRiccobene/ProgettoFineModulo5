using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using static UnityEditor.Searcher.SearcherWindow.Alignment;

public class Player_Input : MonoBehaviour, I_Team
{
    [SerializeField] private int teamNumber = 1;

    public UnityEvent<Vector3, NavMeshAgent> GiveDirectionAndAgent;

    private NavMeshAgent agent;

    private float horizontal;
    private float vertical;
    private Vector3 direction;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        direction = Camera.main.transform.forward * vertical + Camera.main.transform.right * horizontal;
        GiveDirectionAndAgent.Invoke(direction, agent);
    }

    public int GetTeamNumber() => teamNumber;

    public void SetTArget(Transform target) { }
}
