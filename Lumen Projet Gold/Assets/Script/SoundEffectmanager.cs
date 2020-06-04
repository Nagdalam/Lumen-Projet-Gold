using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectmanager : MonoBehaviour
{
    public AudioSource myAudioSource;
    public AudioClip[] audioClips;
    bool victoryPlayed = false;
    void Start()
    {
       
    }

    private void FixedUpdate()
    {
        if (GameManager.playCrystalSound == true && GameManager.audioMuted == false)
        {
            GameManager.playCrystalSound = false;
            myAudioSource.clip = audioClips[0];
            myAudioSource.Play();
        }
        else if (GameManager.playStepSound == true && GameManager.audioMuted == false)
        {
            GameManager.playStepSound = false;
            myAudioSource.clip = audioClips[1];
            myAudioSource.Play();
        }
        else if (GameManager.playIntensificationSound == true && GameManager.audioMuted == false)
        {
            GameManager.playIntensificationSound = false;
            myAudioSource.clip = audioClips[2];
            myAudioSource.Play();
        }
        else if (GameManager.playDarkSound == true && GameManager.audioMuted == false)
        {
            GameManager.playDarkSound = false;
            myAudioSource.clip = audioClips[3];
            myAudioSource.Play();
        }
        else if (GameManager.playVictorySound == true && victoryPlayed == false && GameManager.audioMuted == false)
        {
            victoryPlayed = true;
            GameManager.playVictorySound = false;
            myAudioSource.clip = audioClips[4];
            myAudioSource.Play();
        }

    }
   
    void Update()
    {
        
    }
}
