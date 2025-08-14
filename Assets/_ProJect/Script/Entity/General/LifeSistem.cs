using UnityEngine;
using UnityEngine.Events;

public class LifeSistem : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;

    public UnityEvent <float> OnUpdateHp;
    public UnityEvent OnDead;

    private int hp;

    private void Awake() =>  hp = stats.Hp;

    private void Start()
    {
        OnUpdateHp?.Invoke((float)hp / stats.MaxHp);
    }

    public void UpdateHp(int damage)
    {
        hp += damage;

        OnUpdateHp?.Invoke((float)hp / stats.MaxHp);
        if (IsDead()) OnDead?.Invoke();
    }

    public int GetHp() => hp;
    public int GetMaxHp() => stats.MaxHp;

    private bool IsDead() => hp <= 0;
}
