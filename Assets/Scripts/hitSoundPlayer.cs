using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class hitSoundPlayer : MonoBehaviour
{
    

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player" && PlayerController.instance.enabled == true)
        {
           SoundManager.instance.PlaySound(Random.Range(3,4));
        }
    }
}
