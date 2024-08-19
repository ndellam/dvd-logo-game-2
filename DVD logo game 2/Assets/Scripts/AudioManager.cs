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
        int maxComboForPitchAscension = 10, pitchClamp = 5;
        audioSource.pitch = 1 + (Mathf.Clamp(uiManger.combo, 1, maxComboForPitchAscension) / pitchClamp);
        audioSource.PlayOneShot(clip);
    }
}
