using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    private List<Item_SO> itemlist = new List<Item_SO>();

    private Player_Input player_Input;

    private void Start()
    {
        if (Player_Ui.Instance != null) Player_Ui.Instance.UpdateTextPotionAmountHp(itemlist.Count);
        SetUpAction();
    }

    private void SetUpAction()
    {
        player_Input = GetComponent<Player_Input>();
        player_Input.OnUseItem += UseItem;
    }

    public void UseItem()
    {
        if(itemlist.Count == 0) return;

        itemlist[itemlist.Count - 1].OnUse(gameObject);
        itemlist.RemoveAt(itemlist.Count - 1);

        if (Player_Ui.Instance != null) Player_Ui.Instance.UpdateTextPotionAmountHp(itemlist.Count);
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.TryGetComponent(out Item item))
        {
            itemlist.Add(item.Item_SO);
            if(Player_Ui.Instance != null) Player_Ui.Instance.UpdateTextPotionAmountHp(itemlist.Count);

            Destroy(other.gameObject);
        }
    }

    private void OnDisable()
    {
        player_Input.OnUseItem -= UseItem;

    }
}
