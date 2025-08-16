using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Ui : MonoBehaviour
{
    public static Player_Ui Instance;

    [SerializeField] private Image imageHp;
    [SerializeField] private Image imageMana;

    [SerializeField] private TextMeshProUGUI textPotionAmountHp;

    private void Awake() => Instance = this;

    public void UpdateTextPotionAmountHp(int amount) { if (textPotionAmountHp != null) textPotionAmountHp.text = amount.ToString(); }

    public void UpdateHp(float hp) { if (imageHp != null) imageHp.fillAmount = hp; }

    public void UpdateMana(float mana) { if (imageMana != null) imageMana.fillAmount = mana; }
}
