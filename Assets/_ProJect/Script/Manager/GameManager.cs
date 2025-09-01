using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance {  get; private set; }

    [SerializeField] private CinemachineVirtualCamera virtualCamera;

    private void Awake() => Instance = this;

    public void OnStaticCamera() { if (virtualCamera != null) virtualCamera.enabled = true; }

    public void OffStaticCamera() { if (virtualCamera != null) virtualCamera.enabled = false; }

}
