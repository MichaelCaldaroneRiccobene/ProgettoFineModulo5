using System.Collections;
using UnityEngine;

public class BaseMagic : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] protected GameObject objToDisable;
    [SerializeField] protected float timeLife = 5;
    [SerializeField] protected int damage;

    [SerializeField] protected string idForPool;
    [SerializeField] protected bool isDisableOnImpact = true;

    protected Transform shooter;

    public virtual void OnEnable() => StartCoroutine(LifeTimeRoutione());

    public virtual IEnumerator LifeTimeRoutione()
    {
        yield return new WaitForSeconds(timeLife);
        ReturnToPool();
    }

    public virtual void BasicSetUp(Vector3 position, Quaternion rotation, int damage, Transform shooter)
    {
        objToDisable.transform.position = position;
        objToDisable.transform.rotation = rotation;

        this.damage = damage;
        this.shooter = shooter;
    }

    private void OnCollisionEnter(Collision collision) => OnTouch(collision.collider);

    private void OnTriggerEnter(Collider collider) => OnTouch(collider);

    private void OnTouch(Collider collider)
    {
        if (collider.TryGetComponent(out I_Team team)) team.SetPriorityTarget(shooter);
        if (collider.TryGetComponent(out I_Damageble damageble)) damageble.Damage(-damage);

        if (isDisableOnImpact) ReturnToPool();
    }

    public virtual void ReturnToPool() { if (ManagerPool.Instace != null) ManagerPool.Instace.ReturnToPool(idForPool, objToDisable);}

    public virtual void OnDisable() => StopAllCoroutines();
}
