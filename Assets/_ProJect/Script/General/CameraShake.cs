using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraShake : MonoBehaviour
{
    public static CameraShake Instance {  get; private set; }

    [Header("Setting")]
    [SerializeField] private Transform targetForShakeDistance;

    private CinemachineVirtualCamera virtualCamera;
    private CinemachineBasicMultiChannelPerlin channelPerlin;

    private void Awake() => Instance = this;
    void Start()
    {
        virtualCamera = GetComponent<CinemachineVirtualCamera>();
        channelPerlin = virtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

        channelPerlin.m_AmplitudeGain = 0;
    }

    public void OnCameraShake(Vector3 position, float timer , float maxIntensity, float maxDistance)
    {
        StartCoroutine(OnCameraShakeRoutine(position, timer, maxIntensity, maxDistance));
    }

    private IEnumerator OnCameraShakeRoutine(Vector3 position, float timer,float maxIntensity,float maxDistance)
    {
        float distanceToCamera = Vector3.Distance(position, targetForShakeDistance.position);
        float distanceFactor = Mathf.Clamp01(1 - (distanceToCamera - maxDistance));

        channelPerlin.m_AmplitudeGain = maxIntensity * distanceFactor;
        yield return new WaitForSeconds(timer);

        channelPerlin.m_AmplitudeGain = 0;
    }
}
