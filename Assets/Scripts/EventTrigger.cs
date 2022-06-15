using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventTrigger : MonoBehaviour
{
    public PlayerController player_script;
    public GameObject eventPanel;
    public Text eventText;

    private void Start()
    {
        StartCoroutine(WhenPlayerLose());
    }

    IEnumerator WhenPlayerLose()
    {
        yield return new WaitUntil(() => player_script.isPlayerLose == true);

        //SHOW THE PANEL WHEN PLAYER IS LOOSE
        eventPanel.SetActive(true);
        eventText.text = "Game Over";
    }
}
