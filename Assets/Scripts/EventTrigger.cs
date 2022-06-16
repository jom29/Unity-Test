using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class EventTrigger : MonoBehaviour
{
    
    public PlayerController player_script;
    public GameObject eventPanel;
    public Text eventText;
    public GameObject[] enemies;

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

    IEnumerator WhenPlayerLose()
    {
        //  Debug.Log("Awake:" + SceneManager.GetActiveScene().name);
        yield return new WaitUntil(() => player_script.isPlayerLose == true && SceneManager.GetActiveScene().name.Equals("SampleScene") || 
                                         (TimerEvent.instance.seconds_int + TimerEvent.instance.minutes_int).Equals(0)  && SceneManager.GetActiveScene().name.Equals("SampleScene"));

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






    IEnumerator WhenPlayerWin()
    {
        yield return new WaitUntil(() => ScoreManager.instance.playerScore > 100 && SceneManager.GetActiveScene().name.Equals("SampleScene"));

        //PAUSE TIME WHEN PLAYER WIN
        TimerEvent.instance.pauseTime = true;

        //SHOW THE PANEL WHEN PLAYER IS WIN
        eventPanel.SetActive(true);
        eventText.text = "PLAYER WIN!!!\n" + "PLAYER SCORE:  " +ScoreManager.instance.playerScore;

        //DISABLE AI AND OTHER COMPONENT
        foreach(GameObject enemy in enemies)
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
}
