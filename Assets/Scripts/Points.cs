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
                //TO DETERMINE IF THE PLAYER IS ALREADY EARN GEMS WHEN IT START THE GAME
                //IT WILL BE A GO SIGNAL FROM AI
                if(PlayerController.instance.earnedGems == false)
                {
                    PlayerController.instance.earnedGems = true;
                }

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
            }
            Destroy(gameObject,0.1f);
        }
    }
}
