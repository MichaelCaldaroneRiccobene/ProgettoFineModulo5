
using UnityEngine;

public interface I_Team
{
    public void SetTarget(Transform target);
    public void SetPriorityTarget(Transform target);


    public int GetTeamNumber();
    public Transform GetAllied();


    public bool CanBeFollow();
    public bool HasTarget();
}
        