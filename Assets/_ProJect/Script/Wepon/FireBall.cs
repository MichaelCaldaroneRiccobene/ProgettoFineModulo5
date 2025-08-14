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

    public override void OnDisable()
    {
        StopAllCoroutines();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }
}
