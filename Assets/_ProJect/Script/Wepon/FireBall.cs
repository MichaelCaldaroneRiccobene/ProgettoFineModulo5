using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    [SerializeField] private float speed = 5;
    [SerializeField] private float timeLife = 5;

    private int damage;
    private Transform shoter;

    private IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);

        gameObject.SetActive(false);
    }

    public void OnShoot(Vector3 direction, float shooterVelocity,int damage,Transform shoter)
    {
        this.damage = damage;
        this.shoter = shoter;

        rb.velocity = direction * (speed + shooterVelocity);
    }

    private void OnEnable()
    {
        rb.isKinematic = false;
        rb.velocity = Vector3.zero;

        StartCoroutine(LifeTimeRoutione());
    }

    private void OnDisable()
    {
        StopAllCoroutines();

        rb.velocity = Vector3.zero;
        rb.isKinematic = true;
    }

    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionLife(collision);
        OnCollisionInteract(collision);

        gameObject.SetActive(false);
    }

    private void OnCollisionLife(Collision collision)
    {
        if (collision.collider.TryGetComponent(out EnemyBrain enemy))
        {
            enemy.SetTarget(shoter);
            if (enemy.gameObject.TryGetComponent(out LifeSistem life))
            {
                life.UpdateHp(-damage);
            }
        }
    }

    private void OnCollisionInteract(Collision collision)
    {
        if (collision.collider.TryGetComponent(out I_Interection interection)) interection.Interact();
    }
}
