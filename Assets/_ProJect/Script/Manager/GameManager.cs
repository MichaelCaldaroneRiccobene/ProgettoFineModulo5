using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] private CinemachineVirtualCamera startVirtualCamera;
    [SerializeField] private GameObject pannelUIPlayer;


    private void Awake()
    {
        Instance = this;
    }

    public void OnStart()
    {
      if(startVirtualCamera != null) startVirtualCamera.Priority = 0;
    }
}
