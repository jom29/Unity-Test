using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AI : MonoBehaviour
{

    #region ANIMATION
    [Header("ANIMATION PROPERTIES")]
    public Animator anim;
    private const string IDLE_ANIMATION_BOOL = "idle";
    private const string RUN_ANIMATION_BOOL = "run";
    private const string ATTACK_BOOL = "attack";
    [Space(10)]
    #endregion



    public GameObject smoke;
    public bool obstacle;
    public float speed = 2;
    public bool chasing;
    private GameObject[] patrol_positions;
    private Transform target;
    private float distance;
    private int patrol_id;
    private bool delayTime;

    [Header("Reduce points rate")]
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;


    void Start()
    {
        anim = anim.GetComponent<Animator>();


        GetComponent<MeshRenderer>().material.color = Color.green;

        patrol_positions = GameObject.FindGameObjectsWithTag("patrol");
        //AUTO SETUP TARGET
        if(target == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            target = playerGO.GetComponent<Transform>();
        }
    }

  



    void Raycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                float dist = Vector3.Distance(transform.position, hit.collider.gameObject.GetComponent<Transform>().position);

                if(hit.collider.tag == "enemy" && dist < 5)
                {
                    obstacle = true;
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    StartCoroutine(reset_obstacle());
                }
            }

           
        }
    }

    public void ChangeToBlack()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
    }

    void Update()
    {

        Raycast();


        #region CHASING PLAYER
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);

            if(distance < 5 && chasing == false && TimerEvent.instance.seconds_int != 0)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
                chasing = true;
                StartCoroutine(reset_chasing());
            }


            //SET CHASING TO FALSE WHEN TIMER IS RUNNING OUT
            if((TimerEvent.instance.seconds_int + TimerEvent.instance.minutes_int).Equals(0) && chasing == true)
            {
                chasing = false;
            }


            //CALL DISTANCE ONLY WHEN AI IS CHASING PLAYER
            if(chasing == true)
            {
                transform.LookAt(target.position, new Vector3(0, 0, 0));
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                Debug.DrawLine(transform.position, target.position, Color.yellow);

                distance = Vector3.Distance(transform.position, target.position);
               

                if (distance > 2 && obstacle == false)
                {
                    transform.Translate(Vector3.forward * speed);
                    transform.GetComponent<Rigidbody>().MovePosition(transform.position);
                    AnimateRun();
                  
                }

                else
                {
                  

                    if(ScoreManager.instance.playerScore >= 1 && Time.time > nextFire)
                    {
                        ScoreManager.instance.playerScore--;
                        ScoreManager.instance.UpdateScore();
                        nextFire = Time.time + fireRate;
                        
                    }


                    //SHAKE THE CAR
                  
                     GameObject.Find("Car").GetComponent<Transform>().localEulerAngles = new Vector3(Random.Range(-5, 5), 180, Random.Range(-10, 10));
                    AnimateAttack();



                    //PLAYER LOSE
                    if (PlayerController.instance != null && 
                       PlayerController.instance.earnedGems == true &&
                       ScoreManager.instance.playerScore == 0)
                    {
                        PlayerController.instance.isPlayerLose = true;
                        PlayerController.instance.smoke.SetActive(true);
                        PlayerController.instance.smoke.GetComponent<Transform>().parent = null;
                        PlayerController.instance.enabled = false;
                        target = null;
                    }

                    
                }
            }
        }

        else
        {
            chasing = false;
        }
        #endregion



        //AI PATROLLING
        for (int i = 0; i < patrol_positions.Length; i++)
        {
            if(patrol_positions[i] != null && i == patrol_id && chasing == false && delayTime == false)
            {
               
                transform.LookAt(patrol_positions[i].GetComponent<Transform>(),new Vector3(0,0,0));
                transform.eulerAngles = new Vector3(0,transform.eulerAngles.y,0);
                Debug.DrawLine(transform.position, patrol_positions[i].GetComponent<Transform>().position, Color.red);

                distance = Vector3.Distance(transform.position, patrol_positions[i].GetComponent<Transform>().position);
            

                if(distance > 2 && obstacle == false)
                {
                   transform.Translate(Vector3.forward * speed);
                    AnimateRun();
                }

                else
                {
                    patrol_id = Random.Range(0, patrol_positions.Length);
                    delayTime = true;
                    StartCoroutine(delayTimer());
                    AnimateIdle();
                }
            }
        }
        
    }


    #region IENUMERATOR
    IEnumerator reset_chasing()
    {
        yield return new WaitForSeconds(Random.Range(5,10));
        chasing = false;
        GetComponent<MeshRenderer>().material.color = Color.green;
        AnimateIdle();
    }


    IEnumerator reset_obstacle()
    {
        yield return new WaitForSeconds(2);
        obstacle = false;
        GetComponent<MeshRenderer>().material.color = Color.green;
    }

    IEnumerator delayTimer()
    {
        yield return new WaitForSeconds(2);
        delayTime = false;
    }
    #endregion


    #region ANIMATIONS
    public void AnimateIdle()
    {
        Animate(IDLE_ANIMATION_BOOL);
    }

    public void AnimateRun()
    {
        Animate(RUN_ANIMATION_BOOL);
    }


    public void AnimateAttack()
    {
        Animate(ATTACK_BOOL);
    }


    private void Animate(string boolName)
    {
        DisableOtherAnimations(anim, boolName);
        anim.SetBool(boolName, true);
    }


    private void DisableOtherAnimations(Animator animator, string animation)
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            if (parameter.name != animation)
            {
                animator.SetBool(parameter.name, false);
            }
        }
    }
    #endregion

}
