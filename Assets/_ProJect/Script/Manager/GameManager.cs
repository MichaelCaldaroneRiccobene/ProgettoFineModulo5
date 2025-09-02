using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private string LevelMenu = "Menu";
    [SerializeField] private float velocityForGoToNewLevel = 0.5f;

    private void Awake() => Instance = this;

    public void OnStaticCamera() { if (virtualCamera != null) virtualCamera.enabled = true; }

    public void OffStaticCamera() { if (virtualCamera != null) virtualCamera.enabled = false; }

    public void OnWinLevel() { if (Player_Ui.Instance != null) Player_Ui.Instance.FadeToWinOver(velocityForGoToNewLevel, LevelMenu); }

    public void OnGameOverLevel() { if (Player_Ui.Instance != null) Player_Ui.Instance.FadeToGameOver(velocityForGoToNewLevel, SceneManager.GetActiveScene().buildIndex); }

}
