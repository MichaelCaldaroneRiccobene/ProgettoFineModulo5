using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour, I_Team
{
    public static Player_Controller Instance;
    [Header("Setting CameraShake")]
    [SerializeField] private float durationCameraShake = 0.4f;
    [SerializeField] private float intensityCameraShake = 1.1f;
    [SerializeField] private float distanceCameraShake = 2f;

    [Header("Setting I_Team")]
    [SerializeField] private bool canBeFollow = true;
    [SerializeField] private int teamNumber = 1;

    private void Awake() =>  Instance = this;


    public void UpDatePlayerUI(float hp) { if(Player_Ui.Instance != null) Player_Ui.Instance.UpdateHp(hp); }

    public void ShakeCameraOnHit() => CameraShake.Instance.OnCameraShake(transform.position, durationCameraShake, intensityCameraShake, distanceCameraShake);


    public void OnDead()
    {
        int level = SceneManager.GetActiveScene().buildIndex;
        if(Player_Ui.Instance != null) Player_Ui.Instance.FadeToGameOver(level);

        Player_Input.CanPlayerUseInput = false;
    }

    #region I_Team
    public void SetTarget(Transform target) { }
    public void SetPriorityTarget(Transform target) { }


    public int GetTeamNumber() => teamNumber;
    public Transform GetAllied() => null;
    public Transform GetTarget() => null;


    public bool CanBeFollow() => canBeFollow;
    public bool HasTarget() => false;
    #endregion
}
