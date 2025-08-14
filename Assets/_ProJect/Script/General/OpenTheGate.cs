using System.Collections;
using UnityEngine;

public class OpenTheGate : MonoBehaviour, I_Interection
{
    [SerializeField] private Transform meshFirstGate;
    [SerializeField] private Transform meshSecondGate;

    [SerializeField] private Vector3 openPositionFirstGate = new Vector3(-5, 0, 0);
    [SerializeField] private Vector3 openPositionSecondGate = new Vector3(5,0,0);   


    private Vector3 startPositionFirstGate;
    private Vector3 startPositionSecondGate;

    private bool isOpen = false;

    private void Start()
    {
        if (meshFirstGate == null || meshSecondGate == null) return;

        startPositionFirstGate = meshFirstGate.localPosition;
        startPositionSecondGate = meshSecondGate.localPosition;
    }


    public void Interact()
    {
        if (meshFirstGate == null || meshSecondGate == null) return;

        StopAllCoroutines();
        isOpen = !isOpen;
        StartCoroutine(OpenCloseGateRoutine());
        CameraShake.Instance.OnCameraShake(0.2f, 0.2f);
    }

    private IEnumerator OpenCloseGateRoutine()
    {
        Vector3 currentPositionFirstGate = meshFirstGate.localPosition;
        Vector3 currentPositionSecondGate = meshSecondGate.localPosition;

        Vector3 destinationPositionFirstGate = isOpen? openPositionFirstGate: startPositionFirstGate;
        Vector3 destinationPositionSecondonGate = isOpen ? openPositionSecondGate : startPositionSecondGate;

        float progress = 0;

        while (progress < 1.0f)
        {
            progress += Time.deltaTime;
            meshFirstGate.localPosition = Vector3.Lerp(currentPositionFirstGate, destinationPositionFirstGate, progress);
            meshSecondGate.localPosition = Vector3.Lerp(currentPositionSecondGate, destinationPositionSecondonGate, progress);

            yield return null;
        }

        RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }
}
