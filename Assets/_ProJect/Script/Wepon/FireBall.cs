using UnityEngine;

public class FireBall : BaseWepon
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;

    public void OnShoot(Vector3 direction, float shooterVelocity,int damage,Transform shoter)
    {
        this.damage = damage;
        this.shoter = shoter;

        rb.velocity = direction * (speed + shooterVelocity);
    }

    public override void OnEnable()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        StartCoroutine(LifeTimeRoutione());
    }

    private void SpawnExsplosion()
    {
        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(Parameters_ObjectPool.ExsplosionFireBallObjForpool);
        if (obj == null) return;

        obj.transform.position = transform.position;
        obj.transform.rotation = transform.rotation;

        CameraShake.Instance.OnCameraShake(transform.position,0.25f, 5, 10);
    }

    public override void OnDisable()
    {
        SpawnExsplosion();
        StopAllCoroutines();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
