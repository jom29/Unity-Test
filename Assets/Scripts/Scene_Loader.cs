using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Loader : MonoBehaviour
{
    public void LoadGame(int sceneANumber)
    {
        SceneManager.LoadScene(sceneANumber);
        Time.timeScale = 1;
    }

   public void ExitTheGame()
    {
        Application.Quit();
    }
}
