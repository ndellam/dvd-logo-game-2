using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    public AudioSource footstepSource;

    public AudioClip[] footstepClip;
    public void PlayFootstep()
    {
        footstepSource.PlayOneShot(footstepClip[Random.Range(0, footstepClip.Count())]);
    }

    public void DestroySelf()
    {
        Destroy(this.gameObject);
    }
}
