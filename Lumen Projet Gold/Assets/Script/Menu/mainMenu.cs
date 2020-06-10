using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;
using UnityEngine.UI;

public class mainMenu : MonoBehaviour
{
    bool isMenuOpen;
    Scene sceneLoaded;
    int mySceneID;
    public Image soundImage, musicImage;
    public Sprite audioSprite, audioCrossedSprite, musicSprite, musicCrossedSprite;
    public void PlayGame(int Id)
        {
            SceneManager.LoadScene(Id-1);
        }
    public void ReloadNul()
    {
        SceneManager.LoadScene("Dark2");
    }
    public void OpenAchievementPanel()
    {
        Social.ShowAchievementsUI();
    }

    //public void PlaySelector ()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    //}

        public void LoadLevel()
    {
        SceneManager.LoadScene("Amplif2");
    }

    public void OpenMenu(GameObject menu)
    {
        if (isMenuOpen == false)
        {
            menu.SetActive(true);
            isMenuOpen = true;
        }
        else if (isMenuOpen==true)
        {
            menu.SetActive(false);
            isMenuOpen = false;
        }
    }

    private void Update()
    {
        if (GameManager.musicMuted == false)
        {
            musicImage.sprite = musicSprite;
        }
        if (GameManager.musicMuted == true)
        {
            musicImage.sprite = musicCrossedSprite;
        }
        if (GameManager.audioMuted == false)
        {
            soundImage.sprite = audioSprite;
        }
        if (GameManager.audioMuted == true)
        {
            soundImage.sprite = audioCrossedSprite;
        }
    }

    public void OpenTab(GameObject tab)
    {
        if (GameManager.menuOpen == false)
        {
            GameManager.menuOpen = true;
            tab.SetActive(true);
            
        }
    }

    public void LuoWalk()
    {
        GameManager.numberOfLights = 0;
    }
    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    
    {
        Debug.Log(SceneManager.GetActiveScene().buildIndex);
        SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex)+1);
    }
    public void OntriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag ("Player"))
        {
            Debug.Log("don't touche me");
            SceneManager.LoadScene((SceneManager.GetActiveScene().buildIndex) + 1);
            if (mySceneID > PlayerPrefs.GetInt("levelAt"))
            {
                PlayerPrefs.SetInt("levelAt", mySceneID);
            }

        }
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void MuteAudio()
    {
        if(GameManager.audioMuted == false)
        {
            GameManager.audioMuted = true;
        }
        else if(GameManager.audioMuted == true)
        {
            GameManager.audioMuted = false;
        }
    }

    public void MuteMusic()
    {
        if (GameManager.musicMuted == false)
        {
            musicImage.sprite = musicSprite;
            GameManager.musicMuted = true;
        }
        else if (GameManager.musicMuted == true)
        {
            musicImage.sprite = musicCrossedSprite;
            GameManager.musicMuted = false;
        }
    }


    public void LoadNextSceneName(string name)
    {
        SceneManager.LoadScene(name);
    }

    public void Exit()
    {
        GameManager.menuOpen = false;
    }



}
