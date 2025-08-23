using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSM_ControllerNpc : FSM_Controller
{
    [SerializeField] private float timeUpdateSightRoutine = 0.2f;

    public float TimeUpdateSightRoutine => timeUpdateSightRoutine;
}
