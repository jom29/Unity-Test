using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text score_text;
    public int playerScore;
    public static ScoreManager instance;


    private void Start()
    {
        instance = this;
        score_text = GameObject.Find("PlayerScore UI").GetComponent<Text>();    
    }

    public void UpdateScore ()
    {
        score_text.text = "SCORE: " + playerScore + " points";

    }

}
