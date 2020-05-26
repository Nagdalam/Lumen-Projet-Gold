using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    bool isMenuOpen;
    Scene sceneLoaded;
    int mySceneID;
    public void PlayGame (int Id)
    {
        sceneLoaded = SceneManager.GetActiveScene();
        SceneManager.LoadScene(Id);
        mySceneID = sceneLoaded.buildIndex;
    } 
    //public void PlaySelector ()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    //}
  
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

    public void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void LoadNextScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
    }

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }



}
