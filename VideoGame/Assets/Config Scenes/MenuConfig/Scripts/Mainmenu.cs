using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Mainmenu : MonoBehaviour
{
 
  public void PlayGame()
  {
        PlayerPrefs.SetInt("RegisterNum", 0);
        PlayerPrefs.SetFloat("PuntuacionFinal", 0);
        SceneManager.LoadScene("Loading");
    }


  public void QuitGame()
  {
        Application.Quit(); 
  }

  public void Login()
  {
      
  }

}
