using UnityEngine;

public class FireBall : BaseMagic
{
    [Header("Setting FireBall")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;


    private float durationCameraShake = 0.25f;
    private float powerCameraShake = 5f;
    private float distanceCameraShake = 10;

    public void OnShoot(Vector3 direction) => rb.velocity = direction * speed;

    public override void OnEnable()
    {
        base.OnEnable();

        rb.isKinematic = false;
        rb.velocity = Vector3.zero;
    }

    private void SpawnExsplosion()
    {
        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.ExsplosionFireBallObjForpool);
        if (obj == null) return;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;

        if(CameraShake.Instance != null) CameraShake.Instance.OnCameraShake(transform.position, durationCameraShake, powerCameraShake, distanceCameraShake);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        SpawnExsplosion();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
