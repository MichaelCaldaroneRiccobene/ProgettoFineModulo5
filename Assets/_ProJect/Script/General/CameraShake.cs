using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance {  get; private set; }   

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake()
    {
        if(Instance == null) Instance = this;
    }
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();

        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void OnCameraShake(float intensity, float timer) => StartCoroutine(OnCameraShakeRoutine(intensity, timer));

    private IEnumerator OnCameraShakeRoutine(float intensity, float timer)
    {
        channelPerlin.m_AmplitudeGain = 1.0f;
        yield return new WaitForSeconds(timer);

        channelPerlin.m_AmplitudeGain = 0;
    }
}
