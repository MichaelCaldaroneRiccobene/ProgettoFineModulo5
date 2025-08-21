using System;
using UnityEngine;
using UnityEngine.Events;

public class Transition_OnSitToSitUp : AbstractTransition, I_Interection
{
    [SerializeField] private GameObject pannelInput;

    public UnityEvent OnTriggerSitUp;
 
    private bool conditionMet;

    private void Start()
    {
        pannelInput.SetActive(false);
    }

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState) => conditionMet;

    public void OnCanWalk()
    {
        conditionMet = true;
        pannelInput.SetActive(false);
    }

    public void Interact()
    {
        OnTriggerSitUp?.Invoke();
    }

    public void HideInteractable() { pannelInput.SetActive(false); }
    public void ShowInteractable() 
    {
        if(conditionMet) pannelInput.SetActive(false);
        else pannelInput.SetActive(true);
    }
}
