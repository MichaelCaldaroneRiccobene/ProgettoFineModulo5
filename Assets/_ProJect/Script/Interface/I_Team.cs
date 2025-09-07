using UnityEngine;
public interface I_Team // (So che questa interfaccia e troppo grande, ma me ne sono reso conto troppo tardi, la prossima volta farò più attenzione)
{
    public void ClearTarget();
    public void ClearLastTarget();
    public void ClearAllied();

    public void SetAllied(Transform allied);
    public void SetTarget(Transform target);
    public void SetPriorityTarget(Transform target);
    public void SetLastTarget(Transform lastTarget);


    public int GetTeamNumber();
    public Transform GetAllied();
    public Transform GetTarget();
    public Transform GetLastTarget();


    public bool CanBeFollow();
    public bool HasTarget();
}
