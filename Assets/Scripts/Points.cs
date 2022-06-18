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
            SoundManager.instance.PlaySound(0);
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
                switch(gameObject.GetComponent<gemsID>().color)
                {
                    case "red":
                        carMaterial.color = Color.red;
                    break;

                    case "green":
                        carMaterial.color = Color.green;
                    break;

                    case "blue":
                        carMaterial.color = Color.blue;
                    break;
                }

                
            }

            RespawnManager respawn_script = (RespawnManager)FindObjectOfType(typeof(RespawnManager));

            if(respawn_script != null)
            {
                respawn_script.Reswpawn();
                RotateManager.instance.UpdateTags();
            }
            Destroy(gameObject,0.1f);
        }
    }
}
