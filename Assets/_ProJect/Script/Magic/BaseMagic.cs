using System.Collections;
using UnityEngine;

public class BaseMagic : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] protected GameObject objToDisable;
    [SerializeField] protected float timeLife = 5;
    [SerializeField] protected int damage;

    [SerializeField] protected bool isDestroyOnImpact = true;

    protected Transform shooter;

    public virtual void OnEnable() => StartCoroutine(LifeTimeRoutione());

    public virtual IEnumerator LifeTimeRoutione() { yield return new WaitForSeconds(timeLife); }

    public virtual void BasicSetUp(Vector3 position,Quaternion rotation,int damage,Transform shooter)
    {
        objToDisable.transform.position = position;
        objToDisable.transform.rotation = rotation;

        this.damage = damage;
        this.shooter = shooter;
    }

    public virtual void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out I_Team team)) team.SetTarget(shooter);
        if (other.TryGetComponent(out I_Damageble damageble)) damageble.Damage(-damage);

        if(isDestroyOnImpact) objToDisable.SetActive(false);
    }

    public virtual void OnDisable() => StopAllCoroutines();
}
