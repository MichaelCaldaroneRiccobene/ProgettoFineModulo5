using System.Collections;
using UnityEngine;

public class OpenTheGate : MonoBehaviour, I_Interection
{
    [SerializeField] private Transform button;

    [SerializeField] private Transform meshFirstGate;
    [SerializeField] private Transform meshSecondGate;

    [SerializeField] private Vector3 targetButtonPosition;

    [SerializeField] private Vector3 openRotationFirstGate = new Vector3(-5, 0, 0);
    [SerializeField] private Vector3 openRotationSecondGate = new Vector3(5,0,0);

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

        RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }

    private IEnumerator ButtonAnimatioRoutine()
    {
        float progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * velocityAnimationButton;

            button.localPosition = Vector3.Lerp(startButtonPosition, targetButtonPosition, progress);
            yield return null;
        }

        progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * velocityAnimationButton;

            button.localPosition = Vector3.Lerp(targetButtonPosition, startButtonPosition, progress);
            yield return null;
        }
    }
}
