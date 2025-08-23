using UnityEngine;
using UnityEngine.SceneManagement;

public class UI_Transition : MonoBehaviour
{
    public static UI_Transition Instace;

    [SerializeField] private float velocityTransitionOnStart = 0.5f;

    private CanvasGroup canvasGroup;

    private UI_ShowOrHide showOrHide;
 
    private void Awake()
    {
        Instace = this;

        showOrHide = GetComponent<UI_ShowOrHide>();
        canvasGroup = GetComponent<CanvasGroup>();

        showOrHide.ShowOrHideUI(canvasGroup, velocityTransitionOnStart, 1, 0);
    }

    public void FadeToBlackForLevel(float velocity,string level)
    {
        showOrHide.ShowOrHideWithActionUI(canvasGroup,velocity, canvasGroup.alpha,1, () =>
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

    public void FadeToBlackForLevel(CanvasGroup canvasGroup,float velocity, int level)
    {
        showOrHide.ShowOrHideWithActionUI(canvasGroup, velocity, canvasGroup.alpha, 1, () =>
        {
            SceneManager.LoadScene(level);
        });
    }
}
