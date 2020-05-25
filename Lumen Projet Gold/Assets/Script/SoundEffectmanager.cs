using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundEffectmanager : MonoBehaviour
{
    public List<SoundElement> SeList;
    
    void Start()
    {
        var Audiosource = gameObject.GetComponent<AudioSource>();
        for (int i = 0; i < SeList.Count; i++)
        {
            SeList[i].Index = i;
           Audiosource.pitch = SeList[i].Pitch ;
           Audiosource.volume = SeList[i].Volume;
        } 
    }

    public void PlaySound (int Index, AudioSource source)
    {
        SeList[Index].SE = source.clip;
        source.Play();
    }
    void Update()
    {
        
    }
}
[System.Serializable]
public class SoundElement 
{
    public AudioClip SE;
    public float Volume;
    public float Pitch;
    public int Index;
}
