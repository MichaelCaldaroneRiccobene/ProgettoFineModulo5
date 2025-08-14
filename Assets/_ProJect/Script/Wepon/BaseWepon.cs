using System.Collections;
using UnityEngine;

public class BaseWepon : MonoBehaviour
{
    [SerializeField] protected Transform objToDisable;
    [SerializeField] protected float timeLife = 5;
    [SerializeField] protected int damage;

    protected Transform shoter;

    public virtual IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);

        objToDisable.gameObject.SetActive(false);
    }

    public virtual void OnEnable() => StartCoroutine(LifeTimeRoutione());

    public virtual void OnDisable() => StopAllCoroutines();

    public virtual void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject.name);
        OnCollisionLife(other);
        OnCollisionInteract(other);

        objToDisable.gameObject.SetActive(false);
    }

    public virtual void OnCollisionLife(Collider other)
    {
        if (other.TryGetComponent(out EnemyBrain enemy))
        {
            enemy.SetTarget(shoter);

            if (enemy.gameObject.TryGetComponent(out LifeSistem life)) life.UpdateHp(-damage);
        }
    }

    public virtual void OnCollisionInteract(Collider other) { if (other.TryGetComponent(out I_Interection interection)) interection.Interact(); }
}
