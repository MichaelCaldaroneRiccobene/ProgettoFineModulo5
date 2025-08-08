using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class Player_Input : MonoBehaviour,I_Team
{
    [SerializeField] private int teamNumber = 1;

    public UnityEvent<Vector3, NavMeshAgent> GiveDirectionAndAgent;

    private NavMeshAgent agent;

    private float horizzontal;
    private float vertzontal;
    private Vector3 direction;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        horizzontal = Input.GetAxis("Horizontal");
        vertzontal = Input.GetAxis("Vertical");
    }

    private void FixedUpdate()
    {
        direction = Camera.main.transform.forward * vertzontal + Camera.main.transform.right * horizzontal;
        GiveDirectionAndAgent.Invoke(direction, agent);
    }

    public int GetTeamNumber() => teamNumber;
}
