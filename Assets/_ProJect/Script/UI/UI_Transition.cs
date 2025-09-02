using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Transition : MonoBehaviour
{
    public static UI_Transition Instace {  get; private set; }

    [Header("Setting")]
    [SerializeField] private float velocityTransitionOnStart = 0.5f;

    private CanvasGroup canvasGroup;
    private UI_ShowOrHide showOrHide;

    private void Awake()
    {
        if(Instace != null && Instace != this) { Destroy(gameObject); return; }
        else Instace = this;

        showOrHide = GetComponent<UI_ShowOrHide>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup != null && showOrHide != null) showOrHide.ShowOrHideUI(canvasGroup, velocityTransitionOnStart, 1, 0);
        else Debug.LogError("canvasGroup è " + canvasGroup + " showOrHide è " + showOrHide);
    }

    public void FadeToBlackForLevel(float velocity, string level)
    {
        showOrHide.ShowOrHideWithActionUI(canvasGroup, velocity, canvasGroup.alpha, 1, () =>
        {
            SceneManager.LoadScene(level);
        });
    }

    public void FadeToBlackForLevel(CanvasGroup canvasGroup, float velocity, string level)
    {
        showOrHide.ShowOrHideWithActionUI(canvasGroup, velocity, canvasGroup.alpha, 1, () =>
        {
            SceneManager.LoadScene(level);
        });
    }

    public void FadeToBlackForLevel(CanvasGroup canvasGroup, float velocity, int level)
    {
        showOrHide.ShowOrHideWithActionUI(canvasGroup, velocity, canvasGroup.alpha, 1, () =>
        {
            SceneManager.LoadScene(level);
        });
    }
}
