using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AI_Brtain : MonoBehaviour
{
    public enum StateAI {None,GoOnRandomMove,GoOnTarget}

    public StateAI stateAI;
    
    private NavMeshAgent agent;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        //AI_ControlSingelton.Instance.IsOnTarget(agent);
    }

    private void Update()
    {
        switch (stateAI)
        {
            default:
                break;
            case StateAI.GoOnRandomMove:
                AI_ControlSingelton.Instance.GoOnRandomPosition(agent);
                break;
            case StateAI.GoOnTarget:
                AI_ControlSingelton.Instance.GoOnTarget(agent);
                break;
        }
    }
}
