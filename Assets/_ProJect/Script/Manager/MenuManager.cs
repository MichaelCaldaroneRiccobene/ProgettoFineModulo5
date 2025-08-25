using UnityEngine;
public class MenuManager : MonoBehaviour
{
    public enum PanelType { menu, option, credit }

    [Header("Setting Pannels")]
    [SerializeField] private GameObject[] panels;

    [Header("Setting Name Levels")]
    [SerializeField] private string LevelOne = "Level1";
    [SerializeField] private float velocityTransition = 0.5f;

    private void Start() => SetUpPanel(PanelType.menu);

    private void SetUpPanel(PanelType panelType)
    {
        for (int i = 0; i < panels.Length; i++) panels[i].SetActive(i == (int)panelType);
    }

    public void GoToMenu() => SetUpPanel(PanelType.menu);

    public void GoToOption() => SetUpPanel(PanelType.option);

    public void GoToCredit() => SetUpPanel(PanelType.credit);

    public void GoToLevelOne() => UI_Transition.Instace.FadeToBlackForLevel(velocityTransition, LevelOne);

    public void GoToQuitGame() => Application.Quit();
}
