using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioClip pee;
    public AudioClip bark;
    public AudioClip walk;
    public AudioClip dig;

    AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }
    public void PlaySound(PlayerState state)
    {
        audioSource.loop = false;

        switch (state)
        {
            case PlayerState.PS_CarryBone:
                break;
            case PlayerState.PS_Scared:
                break;
            case PlayerState.PS_Dig:
                audioSource.clip = dig;
                audioSource.Play();
                break;
            case PlayerState.PS_Pee:
                audioSource.clip = pee;
                audioSource.Play();
                break;
            case PlayerState.PS_Bark:
                audioSource.clip = bark;
                audioSource.Play();
                break;
            case PlayerState.PS_GoToBone:
                break;
            case PlayerState.PS_NONE:
                audioSource.clip = null;
                audioSource.Stop();
                break;
        }
    }
    public void PlayWalkSound()
    {
        if ( audioSource.isPlaying && audioSource.clip != null ) return;
        audioSource.loop = true;
        audioSource.clip = walk;
        audioSource.Play();
    }
    public void StopSound()
    {
        if (audioSource.clip == walk && audioSource.isPlaying)
            audioSource.Stop();
    }
}
