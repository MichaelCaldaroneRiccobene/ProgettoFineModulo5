using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player_Inventory : MonoBehaviour
{
    public static Player_Inventory Instance;


    public UnityEvent<int> HpPotion;

    private List<Item_SO> itemlist = new List<Item_SO>();

    private void Awake() => Instance = this;

    public void UseItem()
    {
        if(itemlist.Count == 0) return;

        itemlist[itemlist.Count - 1].OnUse(gameObject);
        itemlist.RemoveAt(itemlist.Count - 1);

        HpPotion?.Invoke(itemlist.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Item item))
        {
            itemlist.Add(item.Item_SO);
            HpPotion?.Invoke(itemlist.Count);

            Destroy(other.gameObject);
        }
    }
}
