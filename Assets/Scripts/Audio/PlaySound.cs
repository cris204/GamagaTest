using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(AudioSource))]
public class PlaySound : MonoBehaviour
{
    public PooleableObject poolObject;
    private AudioSource audioSource;
    private AudioMixer audioMixer;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void PlayAudio(string path, float volume = 1, bool loop = false)
    {
        audioSource.clip = ResourcesManager.Instance.GetAudioClip(path);
        audioSource.volume = volume;
        audioSource.loop = loop;
        audioSource.Play();
        if (!loop) {
            StartCoroutine(WaitToReturnToPool());
        }
    }

    private IEnumerator WaitToReturnToPool()
    {
        if (audioSource.clip != null) {
            yield return new WaitForSeconds(audioSource.clip.length);
        } else {
            yield return null;
        }
        PoolManager.Instance.ReleaseObject(poolObject.path, gameObject);
    }


}