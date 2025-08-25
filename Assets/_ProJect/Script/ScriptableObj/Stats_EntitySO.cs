using UnityEngine;

[CreateAssetMenu(fileName = "Data",menuName = "Entity/Stats")]
public class Stats_EntitySO : ScriptableObject
{
    public string EntityName;

    public int Hp;
    public int MaxHp;

    public int Mana;
    public int MaxMana;

    public int DamageMelee;
    public int DamageRange;
}
