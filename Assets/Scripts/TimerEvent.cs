using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimerEvent : MonoBehaviour
{
    public static TimerEvent instance;
    public bool pauseTime;
    public float timeRemaining = 10;
    public int minutes_int;
    public int seconds_int;


    private void Awake()
    {
        instance = this;
        timerCounts();
    }


    void Update()
    {
        timerCounts();
        if (timeRemaining > 0 && pauseTime.Equals(false))
        {
            timeRemaining -= Time.deltaTime;

            if(GameObject.Find("Timer Text") != null)
            {
                GameObject.Find("Timer Text").GetComponent<Text>().text = minutes_int.ToString() + ": " + seconds_int.ToString(); 
            }
        }
    }

    
    void timerCounts()
    {
        float minutes = Mathf.Floor(timeRemaining / 60);
        float seconds = timeRemaining % 60;
        minutes_int = (int)minutes;
        seconds_int = (int)seconds;
    }
}
