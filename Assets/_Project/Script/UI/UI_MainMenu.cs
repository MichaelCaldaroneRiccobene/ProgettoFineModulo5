using UnityEngine;

public enum PanelMainMenu
{
    Menu = 0,Option = 1,Credit = 2
}

[System.Serializable]
public class MenuPanel
{
    public PanelMainMenu panelType;
    public GameObject panelObj;
}

public class UI_MainMenu : MonoBehaviour
{
    [Header("Setting")]
    [SerializeField] private string level = "Level_1";
    [SerializeField] private float velocityChangeLevel = 0.5f;

    [SerializeField] private MenuPanel[] menuPanels;

    private void Start() => SetUpPannel(PanelMainMenu.Menu);

    private void SetUpPannel(PanelMainMenu panelMenu)
    {
         foreach (MenuPanel panel in menuPanels) panel.panelObj.SetActive(panel.panelType == panelMenu);
    }

    public void GoOnLevel() { if (UI_Transition.Instace != null) UI_Transition.Instace.FadeToBlackForLevel(velocityChangeLevel, level); }

    public void GoOnMenu() => SetUpPannel(PanelMainMenu.Menu);
    public void GoOnOption() => SetUpPannel(PanelMainMenu.Option);
    public void GoOnCredit() => SetUpPannel(PanelMainMenu.Credit);

    public void QuitGame() => Application.Quit();
}
