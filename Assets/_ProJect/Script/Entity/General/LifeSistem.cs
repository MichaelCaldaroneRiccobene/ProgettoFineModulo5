using System;
using UnityEngine;
public class LifeSistem : MonoBehaviour, I_Damageble
{
    [SerializeField] private Stats_EntitySO stats;

    public event Action <float> OnUpdateHp;
    public event Action OnDead;
    public event Action OnHit;

    private int hp;

    private bool isDead;

    private void Awake() => hp = stats.Hp;

    private void Start() => OnUpdateHp?.Invoke((float)hp / stats.MaxHp);

    public void UpdateHp(int amount)
    {
        float tempHp = hp;
        tempHp += amount;

        if (hp > tempHp) OnHit?.Invoke();

        hp = Mathf.Clamp(hp += amount, 0, stats.MaxHp);

        OnUpdateHp?.Invoke((float)hp / stats.MaxHp);

        if (IsDead() && !isDead)
        {
            isDead = true;
            OnDead?.Invoke();
        }
    }


    public bool IsDead() => hp <= 0;

    public void Damage(int damage) => UpdateHp(damage);
}
