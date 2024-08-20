using System.Collections;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioSource musicSource; // Separate audio source for looping music

    public AudioClip gameMusic;

    public UIManager uiManger;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        // Assume musicSource is set up in the Inspector or via code
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

    public void PlaySoundAndChangeMusic(AudioClip soundClip)
    {
        // Stop the looping music
        if (musicSource.isPlaying)
        {
            musicSource.Stop();
        }

        // Play the sound clip
        audioSource.PlayOneShot(soundClip);

        // Start looping the new music clip
        musicSource.clip = gameMusic;
        musicSource.loop = true;
        musicSource.Play();
    }

}
