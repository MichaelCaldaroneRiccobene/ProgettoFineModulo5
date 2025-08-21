using UnityEngine;
using UnityEngine.Events;

public class Allied_Animation : Npc_Animation
{
    [SerializeField] private string parameterTriggerSitUp = "SitUp";

    public UnityEvent OnDoSitUp;

    public void TriggerSitUp() => animator.SetTrigger(parameterTriggerSitUp);
    public void OnSitUp() => OnDoSitUp?.Invoke();
}
