using System.Collections;
using System.Net;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player_Ui : MonoBehaviour
{
    public static Player_Ui Instance;

    [SerializeField] private Image imageHp;
    [SerializeField] private Image imageMana;

    [SerializeField] private string menu = "Menu";
    [SerializeField] private TextMeshProUGUI textPotionAmountHp;

    [SerializeField] private GameObject pannelUIPlayer;
    [SerializeField] private GameObject pannelGameOver;
    [SerializeField] private GameObject pannelWinOver;

    private CanvasGroup canvasGroupPlayerUI;
    private CanvasGroup canvasGroupGameOver;
    private CanvasGroup canvasGroupWinOver;

    private UI_ShowOrHide showOrHide;

    private void Awake() => Instance = this;

    private void Start()
    {
        showOrHide = GetComponent<UI_ShowOrHide>();

        if (pannelUIPlayer != null)
        {
            canvasGroupPlayerUI = pannelUIPlayer.GetComponent<CanvasGroup>();

            if (canvasGroupPlayerUI != null) canvasGroupPlayerUI.alpha = 0;
        }

        if(pannelGameOver != null)
        {
            canvasGroupGameOver = pannelGameOver.GetComponent<CanvasGroup>();

            if (canvasGroupGameOver != null) canvasGroupGameOver.alpha = 0;
        }


        if (pannelWinOver != null)
        {
            canvasGroupWinOver = pannelWinOver.GetComponent<CanvasGroup>();

            if (canvasGroupWinOver != null) canvasGroupWinOver.alpha = 0;
        }
    }

    public void UpdateTextPotionAmountHp(int amount) { if (textPotionAmountHp != null) textPotionAmountHp.text = amount.ToString(); }

    public void UpdateHp(float hp) { if (imageHp != null) imageHp.fillAmount = hp; }

    public void UpdateMana(float mana) { if (imageMana != null) imageMana.fillAmount = mana; }

    public void ShowPlayerUI()
    {
        if (pannelUIPlayer != null && canvasGroupPlayerUI != null)
        {
            showOrHide.ShowOrHideUI(canvasGroupPlayerUI, 1, canvasGroupPlayerUI.alpha,1);
        }
    }
    public void HidePlayerUI()
    {
        if (pannelUIPlayer != null && canvasGroupPlayerUI != null)
        {
            showOrHide.ShowOrHideUI(canvasGroupPlayerUI, 1, canvasGroupPlayerUI.alpha,0);
        }
    }

    public void FadeToGameOver(int level)
    {
        if (UI_Transition.Instace != null) UI_Transition.Instace.FadeToBlackForLevel(canvasGroupGameOver,0.3f, level);
    }

    public void FadeToWinOver()
    {
        if (UI_Transition.Instace != null) UI_Transition.Instace.FadeToBlackForLevel(canvasGroupWinOver, 0.75f, menu);
    }
}
