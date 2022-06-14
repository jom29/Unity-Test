using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    public int score = 10;
    public Material carMaterial;

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            ScoreManager score_manager = (ScoreManager)FindObjectOfType(typeof(ScoreManager));

            if(score_manager != null)
            {
                score_manager.playerScore += score;
                score_manager.UpdateScore();

                switch(gameObject.name)
                {
                    case "Red Gems":
                        carMaterial.color = Color.red;
                    break;

                    case "Green Gems":
                        carMaterial.color = Color.green;
                    break;

                    case "Blue Gems":
                        carMaterial.color = Color.blue;
                    break;
                }


                /*
                 * switch (variable/expression)
{
    case value1:
        // Statements executed if expression(or variable) = value1
        break;
    case value2:
        // Statements executed if expression(or variable) = value1
        break;
    ... ... ... 
    ... ... ... 
    default:
        // Statements executed if no case matches
}
                */
            }
            Destroy(gameObject,0.1f);
        }
    }
}
