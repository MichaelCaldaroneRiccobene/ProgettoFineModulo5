using System.Collections;
using UnityEngine;

public class CubeOfGrass : BaseWeapon
{
    [Header("Setting Position For Animation")]
    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Vector3 midPosition;

    [Header("Setting Velocity For Animation")]
    [SerializeField] private float velocityToEnd = 5;
    [SerializeField] private float velocityToMid = 10;
    [SerializeField] private float velocityToStart = 10;

    private Vector3 originalPosition;

    private void Awake() => SetUpPositions();

    public override void OnEnable()
    {
        base.OnEnable();

        transform.localPosition = originalPosition;
        StartCoroutine(AnimationCubeOfGrassRoutine());
    }

    private void SetUpPositions()
    {
        originalPosition = transform.localPosition;

        endPosition += originalPosition;
        midPosition += originalPosition;
    }

    private IEnumerator AnimationCubeOfGrassRoutine()
    {
        float progress = 0f;
        Vector3 startPosition = transform.localPosition;    

        float distanceToNextPoint = Vector3.Distance(startPosition, midPosition);

        //SpawnPosition To MidPosition
        while ( progress < 1f )
        {
            progress += Time.deltaTime * velocityToMid / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, midPosition, progress);

            yield return null;
        }

        progress = 0;
        startPosition = transform.localPosition;

        distanceToNextPoint = Vector3.Distance(startPosition, originalPosition);

        //MidPosition To SpawnPosition
        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToStart / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, originalPosition, progress);

            yield return null;
        }

        progress = 0;
        startPosition = transform.localPosition;

        distanceToNextPoint = Vector3.Distance(startPosition, endPosition);

        //SpawnPosition To EndPosition
        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToEnd / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, endPosition, progress);

            yield return null;
        }
    }

    public override IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);

        float progress = 0f;
        Vector3 startPosition = transform.localPosition;

        float distanceToNextPoint = Vector3.Distance(startPosition, originalPosition);

        //When Life is Over Go Back in SpawnPosition
        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToStart / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, originalPosition, progress);

            yield return null;
        }
        CameraShake.Instance.OnCameraShake(transform.position, 1, 1.5f, 15);

        RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
        objToDisable.gameObject.SetActive(false);
    }

    public override void OnTriggerEnter(Collider other)
    {
        OnTriggerCollisionLife(other);
        OnTriggerCollisionInteract(other);
    }
}
