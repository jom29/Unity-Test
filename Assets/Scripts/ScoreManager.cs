using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    private Text score_text;
    public int playerScore;
    public float playerScore_BarValue;
    public static ScoreManager instance;

    [Header("Score Status Bar")]
    public Image score_barSprite;



    private void Start()
    {
        instance = this;
        score_text = GameObject.Find("PlayerScore UI").GetComponent<Text>();
      
        if (score_barSprite == null)
        {
           score_barSprite = GameObject.Find("ForeGround").GetComponent<Image>();
        }

        playerScore_BarValue = playerScore * 0.01F;
        score_barSprite.fillAmount = playerScore_BarValue;
    }

    public void UpdateScore ()
    {
        score_text.text = "SCORE: " + playerScore + " points";
        playerScore_BarValue = playerScore * 0.01F;
        score_barSprite.fillAmount = playerScore_BarValue;
    }
}
