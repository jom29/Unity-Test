using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EventTrigger : MonoBehaviour
{
    #region VARIABLES
    //------------------------------------------------------------------
    [Tooltip("isPlayerLose (true or false)\nWait event (Win/Lose))")]
    public PlayerController player_script;
    //=================================================================



    //-----------------------------------------------------------------
    [Tooltip("Enable eventPanel UI\nWhen Player Win or Lose The Round")]
    public GameObject eventPanel;
    //=================================================================
    


    //------------------------------------------------------------------
    [Tooltip("Display Info When \nPlayer Win or Lose The Round")]
    public Text eventText;
    //=================================================================



    //-----------------------------------------------------------------------------
    [Tooltip("Disable some AI behavior\n and components When Player Win or Lose")]
    public GameObject[] enemies;
    //=============================================================================
    #endregion




    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        try
        {
            StartCoroutine(WhenPlayerLose());
        }

        catch (Exception x)
        {
            Debug.LogError("Log Error Found: " + x.ToString());
        }
        StartCoroutine(WhenPlayerWin());
    }


    #region IENUMERATORS
    IEnumerator WhenPlayerWin()
    {
        yield return new WaitUntil(() => ScoreManager.instance.playerScore > 100 && SceneManager.GetActiveScene().name.Equals("SampleScene"));

        //PLAY WIN'S SOUND EFFECTS
        SoundManager.instance.PlaySound(6);
        StartCoroutine(StopAllSounds());

        //PAUSE TIME WHEN PLAYER WIN
        TimerEvent.instance.pauseTime = true;

        //SHOW THE PANEL WHEN PLAYER IS WIN
        eventPanel.SetActive(true);
        eventText.text = "PLAYER WIN!!!\n" + "PLAYER SCORE:  " + ScoreManager.instance.playerScore;

        //DISABLE AI AND OTHER COMPONENT
        foreach (GameObject enemy in enemies)
        {
            enemy.GetComponent<AI>().AnimateIdle();
            enemy.GetComponent<AI>().ChangeToBlack();
            enemy.GetComponent<AI>().smoke.SetActive(true);
            Destroy(enemy.GetComponent<AI>());
            Destroy(enemy.GetComponent<CapsuleCollider>());
            Destroy(enemy.GetComponent<Rigidbody>());
            player_script.enabled = false;
        }
    }

    IEnumerator WhenPlayerLose()
    {
        //  Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
        yield return new WaitUntil(() => player_script.isPlayerLose == true && SceneManager.GetActiveScene().name.Equals("SampleScene") || 
                                         (TimerEvent.instance.seconds_int + TimerEvent.instance.minutes_int).Equals(0)  && SceneManager.GetActiveScene().name.Equals("SampleScene"));


        //PLAY GAME OVER SOUND
        SoundManager.instance.PlaySound(5);
        StartCoroutine(StopAllSounds());


        //SHOW THE PANEL WHEN PLAYER IS LOSE
        eventPanel.SetActive(true);



        if(eventPanel.activeInHierarchy == true)

        eventText.text = "Game Over";
        if(TimerEvent.instance.seconds_int.Equals(0))
        {
            eventText.text += "\nTime is Over";
            PlayerController.instance.smoke.SetActive(true);
            PlayerController.instance.enabled = false;
        }

        if(GameObject.Find("LeftJoystick") != null)
        {
            GameObject.Find("LeftJoystick").SetActive(false);
        }

        else
        {
            Debug.LogError("LeftJoystick not found");
        }

       
        if(GameObject.Find("InGame Btn"))
        {
            GameObject.Find("InGame Btn").SetActive(false);
        }

        else
        {
            Debug.LogError("InGame Btn not Found");
        }
    }

    IEnumerator StopAllSounds()
    {
        yield return new WaitForSeconds(2);
        SoundManager.instance.StopAllSounds();
    }
    #endregion
}
