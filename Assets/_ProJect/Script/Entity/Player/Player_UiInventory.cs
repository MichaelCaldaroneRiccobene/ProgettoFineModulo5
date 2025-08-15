using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player_UiInventory : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textPotionAmountHp;

    public void UpdateTextPotionAmountHp(int amount) => textPotionAmountHp.text = amount.ToString();
}
