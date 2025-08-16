using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeOfGrass : BaseWepon
{
    [SerializeField] private float velocityToEnd = 5;
    [SerializeField] private float velocityToMid = 10;
    [SerializeField] private float velocityToStart = 10;

    [SerializeField] private Vector3 endPosition;
    [SerializeField] private Vector3 midPosition;

    private Vector3 originalPosition;

    private void Awake()
    {
        originalPosition = transform.localPosition;

        endPosition += originalPosition;
        midPosition += originalPosition;
    }

    public override void OnEnable()
    {
        transform.localPosition = originalPosition;
        base.OnEnable();

        StartCoroutine(AnimationCubeOfGrassRoutine());
    }
    private IEnumerator AnimationCubeOfGrassRoutine()
    {
        float progress = 0f;
        Vector3 startPosition = transform.localPosition;    

        float distanceToNextPoint = Vector3.Distance(startPosition, midPosition);


        while ( progress < 1f )
        {
            progress += Time.deltaTime * velocityToMid / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, midPosition, progress);

            yield return null;
        }

        progress = 0;
        startPosition = transform.localPosition;

        distanceToNextPoint = Vector3.Distance(startPosition, originalPosition);

        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToStart / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, originalPosition, progress);

            yield return null;
        }

        progress = 0;
        startPosition = transform.localPosition;


        distanceToNextPoint = Vector3.Distance(startPosition, endPosition);

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

        while (progress < 1f)
        {
            progress += Time.deltaTime * velocityToStart / distanceToNextPoint;
            transform.localPosition = Vector3.Lerp(startPosition, originalPosition, progress);

            yield return null;
        }
        CameraShake.Instance.OnCameraShake(transform.position, 1, 1.5f, 15);

        objToDisable.gameObject.SetActive(false);
    }


    public override void OnTriggerEnter(Collider other)
    {
        OnTriggerCollisionLife(other);
        OnTriggerCollisionInteract(other);
    }

    public override void OnDisable()
    {
        RegenerateNavMesh.Instance.UpdateNaveMeshSurface();
        base.OnDisable();
    }
}
