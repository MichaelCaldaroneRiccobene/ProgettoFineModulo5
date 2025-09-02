using Cinemachine;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instace {  get; private set; }

    [Header("Setting")]
    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    [SerializeField] private string LevelMenu = "Menu";
    [SerializeField] private float velocityForGoToNewLevel = 0.5f;

    private void Awake()
    {
        if (Instace != null && Instace != this) { Destroy(gameObject); return; }
        else Instace = this;
    }

    public void OnStaticCamera() { if (virtualCamera != null) virtualCamera.enabled = true; }

    public void OffStaticCamera() { if (virtualCamera != null) virtualCamera.enabled = false; }

    public void OnWinLevel() { if (Player_Ui.Instance != null) Player_Ui.Instance.FadeToWinOver(velocityForGoToNewLevel, LevelMenu); }

    public void OnGameOverLevel() { if (Player_Ui.Instance != null) Player_Ui.Instance.FadeToGameOver(velocityForGoToNewLevel, SceneManager.GetActiveScene().buildIndex); }

}
