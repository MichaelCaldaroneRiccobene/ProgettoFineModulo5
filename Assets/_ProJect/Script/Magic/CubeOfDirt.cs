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
        StartCoroutine(StartAnimationCubeOfGrassRoutine());
    }

    private void SetUpPositions() => originalScale = transform.localScale;

    private IEnumerator StartAnimationCubeOfGrassRoutine()
    {
        yield return AnimationCubeOfGrassRoutine(scaleOnY);

        if (RegenerateNavMesh.Instance != null) RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }

    private IEnumerator AnimationCubeOfGrassRoutine(float target)
    {
        float progress = 0f;
        Vector3 currentScale = transform.localScale;
        float currentScaleY = currentScale.y;

        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToAnimation;

            currentScale.y = Mathf.Lerp(currentScaleY, target, progress);
            transform.localScale = currentScale;

            yield return null;
        }   
    }

    public override IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);
        yield return AnimationCubeOfGrassRoutine(originalScale.y);

        if (CameraShake.Instace != null) CameraShake.Instace.OnCameraShake(transform.position, durationCameraShake, powerCameraShake, distanceCameraShake);
        ReturnToPool();
    }

    public override void OnDisable()
    {
        base.OnDisable();
        if (RegenerateNavMesh.Instance != null) RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
    }
}
