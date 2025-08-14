using UnityEngine;
using UnityEngine.UI;

public class Player_Ui : MonoBehaviour
{
    [SerializeField] private Image imageHp;
    [SerializeField] private Image imageMana;

    public void UpdateHp(float hp) => imageHp.fillAmount = hp;

    public void UpdateMana(float mana) => imageMana.fillAmount = mana;
}
