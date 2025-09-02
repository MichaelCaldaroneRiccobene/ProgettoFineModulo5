using UnityEngine;
using UnityEngine.UI;

public class Player_Ui : MonoBehaviour
{
    public static Player_Ui Instance {  get; private set; }

    [SerializeField] private Image imageHp;
    [SerializeField] private Image imageMana;

    [SerializeField] private GameObject pannelUIGeneral;
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

        if (pannelUIGeneral != null)
        {
            canvasGroupPlayerUI = pannelUIGeneral.GetComponent<CanvasGroup>();

            if (canvasGroupPlayerUI != null) canvasGroupPlayerUI.alpha = 0;
        }

        if (pannelGameOver != null)
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

    public void UpdateHp(float hp) { if (imageHp != null) imageHp.fillAmount = hp; }

    public void UpdateMana(float mana) { if (imageMana != null) imageMana.fillAmount = mana; }

    public void ShowPlayerUI()
    {
        if (pannelUIGeneral != null && canvasGroupPlayerUI != null)
        {
            showOrHide.ShowOrHideUI(canvasGroupPlayerUI, 1, canvasGroupPlayerUI.alpha, 1);
        }
    }
    public void HidePlayerUI()
    {
        if (pannelUIGeneral != null && canvasGroupPlayerUI != null)
        {
            showOrHide.ShowOrHideUI(canvasGroupPlayerUI, 1, canvasGroupPlayerUI.alpha, 0);
        }
    }

    public void FadeToGameOver(float velocity,int level)
    {
        if (UI_Transition.Instace != null) UI_Transition.Instace.FadeToBlackForLevel(canvasGroupGameOver, velocity, level);
    }

    public void FadeToWinOver(float velocity,string level)
    {
        if (UI_Transition.Instace != null) UI_Transition.Instace.FadeToBlackForLevel(canvasGroupWinOver, velocity, level);
    }
}
