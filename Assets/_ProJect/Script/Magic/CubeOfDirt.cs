using System.Collections;
using UnityEngine;

public class CubeOfDirt : BaseMagic
{
    [Header("Setting Scale Cube")]
    [SerializeField] private float scaleOnY = 8;

    [Header("Setting Velocity For Animation")]
    [SerializeField] private float velocityToAnimation = 10;

    private Vector3 originalScale;

    private float durationCameraShake = 1;
    private float powerCameraShake = 1.5f;
    private float distanceCameraShake = 15;

    private void Awake() => SetUpPositions();

    public override void OnEnable()
    {
        base.OnEnable();
        StartCoroutine(AnimationCubeOfGrassRoutine());
    }

    private void SetUpPositions() => originalScale = transform.localScale;

    private IEnumerator AnimationCubeOfGrassRoutine()
    {
        float progress = 0f;
        Vector3 currentScale = transform.localScale;
        float currentScaleY = currentScale.y;

        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToAnimation;

            currentScale.y = Mathf.Lerp(currentScaleY, scaleOnY, progress);
            transform.localScale = currentScale;

            yield return null;
        }
        if (RegenerateNavMesh.Instance != null) RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }

    public override IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);

        float progress = 0f;
        Vector3 currentScale = transform.localScale;
        float currentScaleY = currentScale.y;

        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToAnimation;

            currentScale.y = Mathf.Lerp(currentScaleY,originalScale.y, progress);
            transform.localScale = currentScale;

            yield return null;
        }

        if (CameraShake.Instance != null) CameraShake.Instance.OnCameraShake(transform.position, durationCameraShake, powerCameraShake, distanceCameraShake);
        objToDisable.gameObject.SetActive(false);
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (RegenerateNavMesh.Instance != null) RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }
}
