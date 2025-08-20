
using UnityEngine;

public interface I_Team
{
    public int GetTeamNumber();

    public void SetTarget(Transform target);

    public void SetTargetPriority(Transform target);
    public Transform GetAllied();

    public bool CanBeFollow();
    public bool HasTarget();
}
        