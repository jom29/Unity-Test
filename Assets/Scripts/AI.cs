using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AI : MonoBehaviour
{
    public GameObject[] patrol_positions;
    public float speed = 2;
    public Transform target;
    private float distance;
    private bool chasing;
    private int patrol_id;
    private bool delayTime;

    void Start()
    {
        patrol_positions = GameObject.FindGameObjectsWithTag("patrol");


        //AUTO SETUP TARGET
        if(target == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            target = playerGO.GetComponent<Transform>();
        }
    }

  
    void Update()
    {
        //GET THE DISTANCE BETWEEN AI AND THE TARGET (PLAYER)
        if(target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);


            //CALL DISTANCE ONLY WHEN AI IS CHASING PLAYER
            if(chasing == true)
            {
                GameObject.Find("DebugText").GetComponent<Text>().text = "Player Distance: " +distance.ToString("#,###,###.####");

            }
        }


        for(int i = 0; i < patrol_positions.Length; i++)
        {
            if(patrol_positions[i] != null && i == patrol_id && chasing == false && delayTime == false)
            {
               
                transform.LookAt(patrol_positions[i].GetComponent<Transform>(),new Vector3(0,0,0));
                transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
                Debug.DrawLine(transform.position, patrol_positions[i].GetComponent<Transform>().position, Color.red);

                distance = Vector3.Distance(transform.position, patrol_positions[i].GetComponent<Transform>().position);
                GameObject.Find("DebugText").GetComponent<Text>().text = "Patrol Distance: " + distance.ToString("#,###,###.####");

                if(distance > 2)
                {
                    transform.Translate(Vector3.forward * speed);
                }

                else
                {
                    patrol_id = Random.Range(0, patrol_positions.Length);
                    delayTime = true;
                    StartCoroutine(delayTimer());
                }
            }
        }
        
    }


    IEnumerator delayTimer()
    {
        yield return new WaitForSeconds(2);
        delayTime = false;
    }


}
