using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;

    public UIManager uiManger;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlayClip(AudioClip clip)
    {
        audioSource.pitch = 1f;
        audioSource.PlayOneShot(clip);
    }

    public void PlayClipAscendingPitchByCombo(AudioClip clip)
    {
        audioSource.pitch = 1 + (Mathf.Clamp(uiManger.combo, 1, 10) / 10);
        audioSource.PlayOneShot(clip);
    }
}
