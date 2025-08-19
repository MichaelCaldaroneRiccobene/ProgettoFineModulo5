using System.Collections;
using UnityEngine;

public class BaseWepon : MonoBehaviour
{
    [SerializeField] protected GameObject objToDisable;
    [SerializeField] protected float timeLife = 5;
    [SerializeField] protected int damage;

    protected Transform shoter;

    public virtual void OnEnable() => StartCoroutine(LifeTimeRoutione());

    public virtual IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);

        gameObject.SetActive(false);
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        OnTriggerCollisionLife(other);
        OnTriggerCollisionInteract(other);

        gameObject.SetActive(false);
    }

    public virtual void OnTriggerCollisionInteract(Collider other)
    {
        if (other.TryGetComponent(out I_Interection interection)) interection.Interact();
    }

    public virtual void OnTriggerCollisionLife(Collider other)
    {
        if (other.TryGetComponent(out I_Team team)) team.SetTarget(shoter);

        if (other.TryGetComponent(out LifeSistem life)) life.UpdateHp(-damage);
    }

    public virtual void OnDisable()
    {
        StopAllCoroutines();
        objToDisable?.SetActive(false);
    }
}
