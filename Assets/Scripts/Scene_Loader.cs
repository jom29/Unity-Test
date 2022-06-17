using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class Scene_Loader : MonoBehaviour
{
    public int indexForSound = 7;
    private int sceneID;

    public void LoadGame(int sceneANumber)
    {
        Time.timeScale = 1;
        sceneID = sceneANumber;
        StartCoroutine(LoadGame_Delay());


        //PLAY SOUND
        try
        {
            if (SoundManager.instance != null)
            {
                SoundManager.instance.PlaySound(indexForSound);
            }
        }

        catch(Exception x)
        {
            Debug.LogError("Sound Index Error: " + x.ToString());
        }
      
    }


    IEnumerator LoadGame_Delay()
    {
        yield return new WaitForSeconds(0.25F);
         SceneManager.LoadScene(sceneID);
      
    }

    public void ExitTheGame()
    {
        Application.Quit();
    }
}
