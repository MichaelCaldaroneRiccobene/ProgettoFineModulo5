using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Transition_OnLostPositionTarget : AbstractTransition
{
    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) => controller.LastTarget == null;
}
