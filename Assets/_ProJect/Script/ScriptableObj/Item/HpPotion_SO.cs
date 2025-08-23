using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "Item/Potions")]
public class HpPotion_SO : Item_SO
{
    [SerializeField] private int ammountHp = 20;

    public override void OnUse(GameObject user) { if (user.TryGetComponent(out LifeSistem life)) life.UpdateHp(ammountHp); } 
}
