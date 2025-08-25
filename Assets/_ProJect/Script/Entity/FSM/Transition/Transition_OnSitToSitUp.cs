using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Transition_OnSitToSitUp : AbstractTransition, I_Interection
{
    [Header("Setting Transition_OnSitToSitUp")]
    [SerializeField] private GameObject pannelInputToPress;
    [SerializeField] private float velocityAnimationButton = 1.5f;

    public UnityEvent OnTriggerSitUp;
    private FSM_Controller controller;
 
    private bool conditionMet;
    private bool isOnInteract;

    private void Start() => pannelInputToPress.SetActive(false);

    public override bool IsConditionMet(FSM_Controller controller, AbstractState ownerState)
    {
        if (this.controller == null) this.controller = controller;

        return conditionMet;
    }

    public void OnCanWalk()
    {
        conditionMet = true;
        pannelInputToPress.SetActive(false);
    }

    public void Interact()
    {
        if (isOnInteract || controller.HasTarget()) return;

        isOnInteract = true;
        OnTriggerSitUp?.Invoke();
        StartCoroutine(ButtonAnimatioRoutine());
    }

    private IEnumerator ButtonAnimatioRoutine()
    {
        Vector3 originLocalScaleCanvas = pannelInputToPress.transform.localScale;
        Vector3 pressLocalScaleCanvas = originLocalScaleCanvas / 2;

        float progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * velocityAnimationButton;

            pannelInputToPress.transform.localScale = Vector3.Lerp(originLocalScaleCanvas, pressLocalScaleCanvas, progress);
            yield return null;
        }

        progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * velocityAnimationButton;

            pannelInputToPress.transform.localScale = Vector3.Lerp(pressLocalScaleCanvas, originLocalScaleCanvas, progress);
            yield return null;
        }

        pannelInputToPress.transform.localScale = originLocalScaleCanvas;
    }


    public void HideInteractable() {if(pannelInputToPress != null) pannelInputToPress.SetActive(false); }
    public void ShowInteractable() 
    {
        if(conditionMet) pannelInputToPress.SetActive(false);
        else pannelInputToPress.SetActive(true);
    }
}
