using UnityEngine;

public class FireBall : BaseWeapon
{
    [Header("Setting FireBall")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;

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

        if(CameraShake.Instance != null) CameraShake.Instance.OnCameraShake(transform.position,0.25f, 5, 10);
    }

    public override void OnDisable()
    {
        base.OnDisable();

        SpawnExsplosion();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
