using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class mainMenu : MonoBehaviour
{
    
   
    public void PlayGame (int Id)
    {
        SceneManager.LoadScene(Id);

    } 
    //public void PlaySelector ()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);
    //}
  
  

    public void QuitGame ()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }


}
