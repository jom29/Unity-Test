using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Fire : MonoBehaviour
{
    public float speed = 0.15f;

    private void Start()
    {
        transform.LookAt(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().transform);
        StartCoroutine(DestroyThisObject());
    }


    private void Update()
    {
        transform.Translate(Vector3.forward * speed);
        this.GetComponent<SphereCollider>().isTrigger = true;


        float dist = Vector3.Distance(GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>().position, transform.position);

        if(dist < 2)
        {
            ScoreManager.instance.playerScore--;
            ScoreManager.instance.UpdateScore();
            SoundManager.instance.PlaySound(9);
            Destroy(this.gameObject);
        }
    }


    IEnumerator DestroyThisObject()
    {
        yield return new WaitForSeconds(3);
        Destroy(this.gameObject);
    }
}
