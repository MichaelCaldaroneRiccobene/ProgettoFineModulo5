using UnityEngine;
using UnityEngine.Events;

public class LifeSistem : MonoBehaviour
{
    [SerializeField] private Stats_EntitySO stats;

    public UnityEvent OnDead;

    private int hp;

    private void Awake() =>  hp = stats.Hp;

    private void Update()
    {
        Debug.Log(gameObject.name + " " + hp);
    }

    public void UpdateHp(int damage)
    {
        hp += damage;

        if(IsDead()) OnDead?.Invoke();
    }

    public int GetHp() => hp;
    public int GetMaxHp() => stats.MaxHp;

    private bool IsDead() => hp <= 0;
}
