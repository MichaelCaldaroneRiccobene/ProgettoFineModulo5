using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    [SerializeField] private AudioSource soundVFX;

    [SerializeField] private float minPitch = 0.9f;
    [SerializeField] private float maxPitch = 1.2f;

    private void Awake()
    {
        Instance = this;
    }

    public void PlaySoundVFX(AudioClip audioClip,Transform transform,float volume, bool randomPitch)
    {
        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(NameMixManager.SoundFX3D);
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        audioSource.transform.position = transform.position;
        SetUpAudio(audioSource, audioClip, volume, randomPitch);
    }

    public void PlaySoundVFX(AudioClip audioClip,float volume, bool randomPitch)
    {
        GameObject obj = ManagerPool.Instace.GetGameObjFromPool(NameMixManager.SoundFX);
        AudioSource audioSource = obj.GetComponent<AudioSource>();

        SetUpAudio(audioSource, audioClip, volume, randomPitch);
    }

    private void SetUpAudio(AudioSource audioSource, AudioClip audioClip,float volume, bool randomPitch)
    {
        audioSource.clip = audioClip;

        audioSource.volume = volume;

        if (randomPitch) audioSource.pitch = Random.Range(minPitch, maxPitch);
        else audioSource.pitch = 1;

        audioSource.Play();

        float clipLengh = audioSource.clip.length;
        

        StartCoroutine(LifeTimeRoutione(audioSource, clipLengh));
    }

    public virtual IEnumerator LifeTimeRoutione(AudioSource audioSource, float timeLife)
    {
        yield return new WaitForSeconds(timeLife);
        audioSource.gameObject.SetActive(false);
    }
}
