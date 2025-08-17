using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class State_Wait : AbstractState
{
    public override void StateEnter() { Debug.Log("Entrato in State Wait"); }

    public override void StateExit() { Debug.Log("Uscito dallo State Wait"); }

    public override void StateUpdate() { }
}
