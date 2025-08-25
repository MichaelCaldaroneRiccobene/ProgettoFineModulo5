using UnityEngine;

public class MenuInGameManager : MonoBehaviour
{
    public static MenuInGameManager Instance;

    public enum PanelType { menu, option, credit, none }

    [Header("Setting Pannels")]
    [SerializeField] private GameObject[] panels;

    [Header("Setting Name Levels")]
    [SerializeField] private string menu = "Menu";
    [SerializeField] private float velocityTransition = 0.5f;

    private void Awake() => Instance = this;

    private void Start() => SetUpPanel(PanelType.none);

    private void SetUpPanel(PanelType panelType)
    {
        for (int i = 0; i < panels.Length; i++) panels[i].SetActive(i == (int)panelType);
    }

    public void GoToOpenMenu()
    {
        SetUpPanel(PanelType.menu);
        Player_Input.CanPlayerUseInput = false;
        Time.timeScale = 0;
    }

    public void GoToResume()
    {
        SetUpPanel(PanelType.none);
        Player_Input.CanPlayerUseInput = true;
        Time.timeScale = 1;
    }


    public void GoToMainMenu()
    {
        SetUpPanel(PanelType.none);
        Player_Input.CanPlayerUseInput = false;
        Time.timeScale = 1;

        UI_Transition.Instace.FadeToBlackForLevel(velocityTransition, menu);
    }

    public void GoToMenu() => SetUpPanel(PanelType.menu);

    public void GoToOption() => SetUpPanel(PanelType.option);

    public void GoToCredit() => SetUpPanel(PanelType.credit);

    public void GoToQuitGame() => Application.Quit();
}
