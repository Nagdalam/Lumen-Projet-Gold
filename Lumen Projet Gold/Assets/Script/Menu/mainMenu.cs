using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GooglePlayGames;

public class mainMenu : MonoBehaviour
{
    bool isMenuOpen;
    Scene sceneLoaded;
    int mySceneID;
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

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }

    public void Exit()
    {
        GameManager.menuOpen = false;
    }



}
