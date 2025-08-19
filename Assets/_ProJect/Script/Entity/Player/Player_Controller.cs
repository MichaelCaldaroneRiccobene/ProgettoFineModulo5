using UnityEngine;
using UnityEngine.SceneManagement;

public class Player_Controller : MonoBehaviour, I_Team
{
    public static Player_Controller Instance;

    [SerializeField] private int teamNumber = 1;

    private void Awake() =>  Instance = this;


    public void UpDatePlayerUI(float hp) { if(Player_Ui.Instance != null) Player_Ui.Instance.UpdateHp(hp); }

    public void ShakeCameraOnHit() => CameraShake.Instance.OnCameraShake(transform.position, 0.25f, 1, 2);


    public int GetTeamNumber() => teamNumber;

    public void SetTarget(Transform target) { }

    public void OnDead()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        Debug.Log("I Am Dead");
    }

    public bool HasTarget() => false;

    public bool CanBeFollow() => true;

    public Transform GetAllied() => null;
}
