using System.Collections;
using UnityEngine;

public class InterectionOpenAndClose : MonoBehaviour, I_Interection
{
    [SerializeField] private Transform mesh;
    [SerializeField] private Vector3 openRotation;
    [SerializeField] private float velocityOpenCloseDoor = 5;

    private Quaternion startRotation;
    private Quaternion targetRotation;

    private bool isOpen = false;

    private void Start()
    {
        startRotation = mesh.rotation;
        targetRotation = startRotation * Quaternion.Euler(openRotation);
    }

    public void Interact()
    {
        isOpen = !isOpen;
        StopAllCoroutines();

        StartCoroutine(OpenAndCloseRountine(isOpen ? targetRotation : startRotation));
    }


    private IEnumerator OpenAndCloseRountine(Quaternion targetRotation)
    {
        Quaternion startRotation = mesh.rotation;
        float progress = 0; 

        while (progress < 1.0f)
        {
            progress += Time.deltaTime * velocityOpenCloseDoor;
            mesh.rotation = Quaternion.Lerp(startRotation, targetRotation, progress);
            yield return null;
        }

        RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }
}
