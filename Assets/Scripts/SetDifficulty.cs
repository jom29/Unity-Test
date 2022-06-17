using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetDifficulty : MonoBehaviour
{
    private int valueToSend;

    public void SetTimer(int value)
    {
        DontDestroyOnLoad(gameObject);
        valueToSend = value;
        StartCoroutine(SetTimer_WhenScript_isActive());
    }


    IEnumerator SetTimer_WhenScript_isActive()
    {
        yield return new WaitUntil(() => TimerEvent.instance != null);
        TimerEvent.instance.timeRemaining = valueToSend;
    }

}
