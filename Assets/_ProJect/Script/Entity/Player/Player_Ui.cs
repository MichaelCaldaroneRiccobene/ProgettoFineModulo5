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

    [SerializeField] private TextMeshProUGUI textPotionAmountHp;

    [SerializeField] private GameObject pannelUIPlayer;

    private Coroutine coroutinePlayerUI;
    private CanvasGroup canvasGroupPlayerUI;

    private void Awake() => Instance = this;

    private void Start()
    {
        if (pannelUIPlayer != null)
        {
            canvasGroupPlayerUI = pannelUIPlayer.GetComponent<CanvasGroup>();

            if (canvasGroupPlayerUI != null) canvasGroupPlayerUI.alpha = 0;
        }
    }

    public void UpdateTextPotionAmountHp(int amount) { if (textPotionAmountHp != null) textPotionAmountHp.text = amount.ToString(); }

    public void UpdateHp(float hp) { if (imageHp != null) imageHp.fillAmount = hp; }

    public void UpdateMana(float mana) { if (imageMana != null) imageMana.fillAmount = mana; }

    public void ShowPlayerUI()
    {
        if (pannelUIPlayer != null && canvasGroupPlayerUI != null)
        {
            if (coroutinePlayerUI != null) StopCoroutine(coroutinePlayerUI);
            coroutinePlayerUI = StartCoroutine(AToBRoutine(canvasGroupPlayerUI, 1, canvasGroupPlayerUI.alpha, 1));
        }
    }
    public void HidePlayerUI()
    {
        if (pannelUIPlayer != null && canvasGroupPlayerUI != null)
        {
            if (coroutinePlayerUI != null) StopCoroutine(coroutinePlayerUI);
            coroutinePlayerUI = StartCoroutine(AToBRoutine(canvasGroupPlayerUI, 1, canvasGroupPlayerUI.alpha, 0));
        }
    }

    private IEnumerator AToBRoutine(CanvasGroup canvasGroup, float velocity, float startPoint, float endPoint)
    {
        float currentA = startPoint;
        if (velocity <= 0) velocity = 1;

        float progress = 0;
        while (progress < 1)
        {
            progress += Time.deltaTime * velocity;

            canvasGroup.alpha = Mathf.Lerp(currentA, endPoint, progress);
            yield return null;
        }
        canvasGroup.alpha = endPoint;
    }
}
