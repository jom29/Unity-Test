using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text score_text;
    public int playerScore;

    private void Start()
    {
        score_text = GameObject.Find("PlayerScore UI").GetComponent<Text>();    
    }

    public void AddScore (int score)
    {
        score_text.text = "SCORE: " + score + " points";
    }

}
