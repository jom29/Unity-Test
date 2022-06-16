using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InGameUI : MonoBehaviour
{
    public GameObject[] CanvasUI;
   public void Pause()
   {
        Time.timeScale = 0;
        

        for(int i = 0; i < CanvasUI.Length; i++)
        {
            CanvasUI[i].SetActive(false);
        }
   }

   public void Resume()
    {
        Time.timeScale = 1;
        for (int i = 0; i < CanvasUI.Length; i++)
        {
            CanvasUI[i].SetActive(true);
        }
    }

}
