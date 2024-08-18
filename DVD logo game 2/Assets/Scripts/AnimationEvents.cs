using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public AudioSource footstepSource;

    public AudioClip footstepClip;
    public void PlayFootstep()
    {
        footstepSource.PlayOneShot(footstepClip);
    }
}
