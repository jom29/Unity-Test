using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTrigger : MonoBehaviour
{
    
    public PlayerController player_script;
    public GameObject eventPanel;
    public Text eventText;
    public GameObject[] enemies;

    private void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        StartCoroutine(WhenPlayerLose());
        StartCoroutine(WhenPlayerWin());
    }

    IEnumerator WhenPlayerLose()
    {
        yield return new WaitUntil(() => player_script.isPlayerLose == true || TimerEvent.instance.seconds_int.Equals(0));

        //SHOW THE PANEL WHEN PLAYER IS LOSE
        eventPanel.SetActive(true);
        eventText.text = "Game Over";

        if(TimerEvent.instance.seconds_int.Equals(0))
        {
            eventText.text += "\nTime is Over";
            PlayerController.instance.smoke.SetActive(true);
            PlayerController.instance.enabled = false;
        }

        GameObject.Find("LeftJoystick").SetActive(false);
    }










    IEnumerator WhenPlayerWin()
    {
        yield return new WaitUntil(() => ScoreManager.instance.playerScore > 100);

        //PAUSE TIME WHEN PLAYER WIN
        TimerEvent.instance.pauseTime = true;

        //SHOW THE PANEL WHEN PLAYER IS WIN
        eventPanel.SetActive(true);
        eventText.text = "PLAYER WIN!!!\n" + "PLAYER SCORE:  " +ScoreManager.instance.playerScore;

        //DISABLE AI AND OTHER COMPONENT
        foreach(GameObject enemy in enemies)
        {
            enemy.GetComponent<AI>().ChangeToBlack();
            enemy.GetComponent<AI>().smoke.SetActive(true);
            Destroy(enemy.GetComponent<AI>());
            Destroy(enemy.GetComponent<CapsuleCollider>());
            Destroy(enemy.GetComponent<Rigidbody>());
            player_script.enabled = false;
        }
    }
}
