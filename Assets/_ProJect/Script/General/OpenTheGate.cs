using System.Collections;
using UnityEngine;

public class OpenTheGate : MonoBehaviour, I_Interection, I_Damageble
{
    [Header("Setting")]
    [SerializeField] private Transform button;
    [SerializeField] private GameObject pannelInputToPress;

    [SerializeField] private Transform meshFirstGate;
    [SerializeField] private Transform meshSecondGate;

    [SerializeField] private Vector3 targetButtonPosition;

    [SerializeField] private Vector3 openRotationFirstGate = new Vector3(-5, 0, 0);
    [SerializeField] private Vector3 openRotationSecondGate = new Vector3(5,0,0);

    private UI_ShowOrHide uI_ShowOrHide;
    private CanvasGroup canvasGroupInput;

    private Vector3 startButtonPosition;

    private Quaternion startRotationFirstGate;
    private Quaternion targetRotationFirstGate;

    private Quaternion startRotationSecondGate;
    private Quaternion targetRotationSecondGate;

    private float velocityAnimationButton = 2;

    private float timeShakeCameraWhenOpenGate = 1;
    private float intesityShakeCameraWhenOpenGate = 0.5f;
    private float maxDistanceShakeCameraWhenOpenGate = 20;

    private bool isOpen = false;
    private bool isMoving = false;

    private void Start()
    {
        if(pannelInputToPress != null)
        {
            canvasGroupInput = pannelInputToPress.GetComponent<CanvasGroup>();
            if (canvasGroupInput != null)
            {
                canvasGroupInput.alpha = 0;
                uI_ShowOrHide = GetComponent<UI_ShowOrHide>();
            }
        }

        if (meshFirstGate == null || meshSecondGate == null || button == null) return;

        startButtonPosition = button.localPosition;

        startRotationFirstGate = meshFirstGate.localRotation;
        startRotationSecondGate = meshSecondGate.localRotation;

        targetRotationFirstGate = startRotationFirstGate * Quaternion.Euler(openRotationFirstGate);
        targetRotationSecondGate = startRotationFirstGate * Quaternion.Euler(openRotationSecondGate);
    }


    public void Interact()
    {
        if (meshFirstGate == null || meshSecondGate == null || button == null || isMoving) return;
        isOpen = !isOpen;
        isMoving = true;

        StartCoroutine(OpenCloseGateRoutine());
        StartCoroutine(ButtonAnimatioRoutine());
        CameraShake.Instance.OnCameraShake(meshFirstGate.position, timeShakeCameraWhenOpenGate, intesityShakeCameraWhenOpenGate,maxDistanceShakeCameraWhenOpenGate);
    }

    private IEnumerator OpenCloseGateRoutine()
    {
        Quaternion currentRotationFirstGate = meshFirstGate.localRotation;
        Quaternion currentRotationSecondGate = meshSecondGate.localRotation;

        Quaternion destinationRotationFirstGate = isOpen? targetRotationFirstGate : startRotationFirstGate;
        Quaternion destinationRotationSecondonGate = isOpen ? targetRotationSecondGate : startRotationSecondGate;

        float progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime;

            meshFirstGate.localRotation = Quaternion.Lerp(currentRotationFirstGate, destinationRotationFirstGate, progress);
            meshSecondGate.localRotation = Quaternion.Lerp(currentRotationSecondGate, destinationRotationSecondonGate, progress);

            yield return null;
        }

        meshFirstGate.localRotation = destinationRotationFirstGate;
        meshSecondGate.localRotation = destinationRotationSecondonGate;

        isMoving = false;
        RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
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
            button.localPosition = Vector3.Lerp(startButtonPosition, targetButtonPosition, progress);
            yield return null;
        }

        progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * velocityAnimationButton;

            pannelInputToPress.transform.localScale = Vector3.Lerp(pressLocalScaleCanvas, originLocalScaleCanvas, progress);
            button.localPosition = Vector3.Lerp(targetButtonPosition, startButtonPosition, progress);
            yield return null;
        }

        pannelInputToPress.transform.localScale = originLocalScaleCanvas;
        button.localPosition = startButtonPosition;
    }


    public void ShowInteractable() 
    { 
        if(canvasGroupInput != null)
        {
            uI_ShowOrHide.ShowOrHideUI(canvasGroupInput, 3, canvasGroupInput.alpha, 1);
        }
    }

    public void HideInteractable()
    {
        if (canvasGroupInput != null)
        {
            uI_ShowOrHide.ShowOrHideUI(canvasGroupInput, 3, canvasGroupInput.alpha, 0);
        }
    }

    public void Damage(int damage) => Interact();
}
