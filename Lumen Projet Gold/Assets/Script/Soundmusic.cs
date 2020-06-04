using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Soundmusic: MonoBehaviour {
    public AudioSource myAudioSource;
	// Use this for initialization
	void Start () {
		
	}

    //Play Global
    private static Soundmusic instance = null;
    public static Soundmusic Instance
    {
        get { return instance; }
    }

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(this.gameObject);
    }
    //Play Gobal End

    // Update is called once per frame
    void Update () {
        if (GameManager.musicMuted == true)
        {
            myAudioSource.volume = 0;
        }
        if (GameManager.musicMuted == false)
        {

            myAudioSource.volume = 0.2f;
        }
    }
}
