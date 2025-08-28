using UnityEngine;
using UnityEngine.UI;
public class MenuManager : MonoBehaviour
{
    public enum PanelType { menu, option, credit }

    [Header("Setting Pannels")]
    [SerializeField] private GameObject[] panels;

    [Header("Setting Name Levels")]
    [SerializeField] private string LevelOne = "Level1";
    [SerializeField] private float velocityTransition = 0.5f;

    [Header("Setting Sliders")]
    [SerializeField] private Slider masterSound;
    [SerializeField] private Slider soundFXSound;
    [SerializeField] private Slider musicSound;

    [SerializeField] private AudioClip[] pressButtons;
    [SerializeField] private float volumeBotton = 0.5f;
    [SerializeField] private bool isRandomPitch = true;

    private void Start()
    {
        SetUpPanel(PanelType.menu);
        LoadSlidersVolume();
    }

    private void SetUpPanel(PanelType panelType)
    {
        for (int i = 0; i < panels.Length; i++) panels[i].SetActive(i == (int)panelType);
    }

    public void GoToMenu()
    {
        PlaySoundBotton();
        SetUpPanel(PanelType.menu);
    }

    public void GoToOption()
    {
        PlaySoundBotton();
        SetUpPanel(PanelType.option);
    }

    public void GoToCredit()
    {
        PlaySoundBotton();
        SetUpPanel(PanelType.credit);
    }

    public void GoToLevelOne()
    {
        PlaySoundBotton();
        UI_Transition.Instace.FadeToBlackForLevel(velocityTransition, LevelOne);
    }

    private void PlaySoundBotton()
    {
        SoundManager.Instance.PlaySoundVFX(pressButtons[Random.Range(0, pressButtons.Length)], volumeBotton, isRandomPitch);
    }


    public void SaveSlidersMaster() => PlayerPrefs.SetFloat(NameMixManager.MasterVolume, masterSound.value);
    public void SaveSlidersSoundFX() => PlayerPrefs.SetFloat(NameMixManager.SoundFX, soundFXSound.value);
    public void SaveSlidersMusic() => PlayerPrefs.SetFloat(NameMixManager.Music, musicSound.value);


    public void LoadSlidersVolume()
    {
        masterSound.value = PlayerPrefs.GetFloat(NameMixManager.MasterVolume);
        soundFXSound.value = PlayerPrefs.GetFloat(NameMixManager.SoundFX);
        musicSound.value = PlayerPrefs.GetFloat(NameMixManager.Music);
    }

    public void GoToQuitGame()
    {
        PlaySoundBotton();
        Application.Quit();
    }
}
