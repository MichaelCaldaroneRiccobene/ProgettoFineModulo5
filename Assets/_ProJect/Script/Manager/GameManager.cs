using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera startVirtualCamera;


    public static GameManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void OnStart()
    {
      if(startVirtualCamera != null)  startVirtualCamera.Priority = 0;
    }
}
