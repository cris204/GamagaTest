using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{

    public AudioMixer audioMixer;

    public float GetVolume()
    {
        audioMixer.GetFloat("volume", out float value);
        return Remap(value, -80f, 0f, 0f, 1f);
    }

    public void SetVolume(float value)
    {
        audioMixer.SetFloat("volume", Mathf.Log10(value)*20);
    }

    float Remap(float value, float min1, float max1, float min2, float max2)
    {
        return min2 + (value - min1) * (max2 - min2) / (max1 - min1);
    }
}
