using UnityEngine;

public class State_Wait : AbstractState
{
    public override void StateEnter() { if (controller.CanSeeDebug) Debug.Log("Entrato in State Wait"); }

    public override void StateExit() { if (controller.CanSeeDebug) Debug.Log("Uscito dallo State Wait"); }

    public override void StateUpdate() { }
}
