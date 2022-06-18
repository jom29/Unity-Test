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
    private const string CASTING_BOOL = "casting";
    [Space(10)]
    #endregion

    #region COMMON VARIABLES
    [Header("Common Variables")]
    public GameObject smoke;
    public bool obstacle;
    public float speed = 2;
    public bool chasing;
    private GameObject[] patrol_positions;
    private Transform target;
    private float distance;
    private int patrol_id;
    private bool delayTime;
    [Space]
    #endregion

    #region RATES AND ATTACK PROPERTIES
    [Header("Reduce points rate")]
    public float fireRate = 0.5F;
    private float nextFire = 0.0F;

    [Header("Sphere Shake rate")]
    public float shakeRate = 0.5F;
    public float nextShake = 0.0F;
    private Transform avatarTarget;

    [Space]

    [Header("Shoot Properties")]
    public bool isShooting;
    public float shootRate = 3;
    public float shootNext = 0.0F;
    public bool seen_by_AI;
    public GameObject spell_prefab;
    public Transform spell_respawner;
    #endregion

    void Start()
    {
        anim = anim.GetComponent<Animator>();


        GetComponent<MeshRenderer>().material.color = Color.green;

        patrol_positions = GameObject.FindGameObjectsWithTag("patrol");
        //AUTO SETUP TARGET
        if (target == null)
        {
            GameObject playerGO = GameObject.FindGameObjectWithTag("Player");
            target = playerGO.GetComponent<Transform>();
        }



        #region ADD SPHERE SETUP 
        shakeRate = 0.1F;
        avatarTarget = transform.Find("Avatar");
        if (avatarTarget != null)
        {
            avatarTarget.gameObject.AddComponent<SphereCollider>();
            avatarTarget.gameObject.GetComponent<SphereCollider>().isTrigger = true;
            Vector3 centerSetting = new Vector3(0, 1, 0);
            avatarTarget.gameObject.GetComponent<SphereCollider>().center = new Vector3(0, 1, 0);
        }
        #endregion
    }

    #region MAIN
    void Update()
    {
        ShakeTheSphere();
        Raycast();
        AIPatrol();
        ChasingPlayer();
    }
    #endregion

    #region FUNCTIONS
    //------------------------------------------------------------------------------------
    //RAYCAST TO AVOID OBSTACLES (AVOID OBJECTS WITH COLLIDERS)
    void Raycast()
    {
        RaycastHit hit;
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider != null)
            {
                float dist = Vector3.Distance(transform.position, hit.collider.gameObject.GetComponent<Transform>().position);

                if (hit.collider.tag.Equals("enemy") && dist < 5)
                {
                    obstacle = true;
                    GetComponent<MeshRenderer>().material.color = Color.red;
                    StartCoroutine(reset_obstacle());
                }

                //SEEN BY AI 
                if (hit.collider.tag.Equals("Player") && seen_by_AI == false && distance > 2)
                {
                    seen_by_AI = true;
                    AnimateCasting();
                    StartCoroutine(reset_Seen_By_AI(Random.Range(3,5)));
                }

                //SHOOT, INSTANTIATE SPELLS
                if(hit.collider.tag.Equals("Player") && distance > 2 && Time.time > shootNext)
                {
                    if(spell_prefab != null)
                    {
                        GameObject clone;
                        clone = Instantiate(spell_prefab, spell_respawner.position, spell_respawner.rotation) as GameObject;
                    }
                    SoundManager.instance.PlaySound(8);
                    shootNext = Time.time + shootRate;
                }
            }
        }
    }


    //USE THIS TO MAKE TRIGGER WORKS
    void ShakeTheSphere()
    {
        if (avatarTarget != null)
            if (Time.time > nextShake)
            {
                avatarTarget.gameObject.GetComponent<SphereCollider>().radius = Random.Range(0.1f, 1.5f);
                nextShake = Time.time + shakeRate;
            }
    }


    //DEBUG MARKER FOR AI
    public void ChangeToBlack()
    {
        GetComponent<MeshRenderer>().material.color = Color.black;
    }

    //PATROL SYSTEM
    void AIPatrol()
    {
        //AI PATROLLING
        for (int i = 0; i < patrol_positions.Length; i++)
        {
            if (patrol_positions[i] != null && i == patrol_id && chasing == false && delayTime == false)
            {

                transform.LookAt(patrol_positions[i].GetComponent<Transform>(), new Vector3(0, 0, 0));
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                Debug.DrawLine(transform.position, patrol_positions[i].GetComponent<Transform>().position, Color.red);

                distance = Vector3.Distance(transform.position, patrol_positions[i].GetComponent<Transform>().position);


                if (distance > 2 && obstacle == false)
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


    //CHASING SYSTEM
    void ChasingPlayer()
    {
        #region CHASING PLAYER
        if (target != null)
        {
            distance = Vector3.Distance(transform.position, target.position);


            #region CHANGE CAR SKIN & SET CHASING ACTIVE
            if (distance < 5 && chasing == false && (TimerEvent.instance.minutes_int + TimerEvent.instance.seconds_int) != 0)
            {
                GetComponent<MeshRenderer>().material.color = Color.blue;
                chasing = true;
                StartCoroutine(reset_chasing());
            }
            #endregion



            //SET CHASING TO FALSE WHEN TIMER IS RUNNING OUT
            if ((TimerEvent.instance.seconds_int + TimerEvent.instance.minutes_int).Equals(0) && chasing == true)
            {
                chasing = false;
            }


            //CHASE THE PLAYER
            if (chasing == true)
            {
                Debug.DrawLine(transform.position, target.position, Color.yellow);

                // MOVE FORWARD TO PLAYER
                transform.LookAt(target.position, new Vector3(0, 0, 0));
                transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0);
                distance = Vector3.Distance(transform.position, target.position);

                // TRANSLATE POSITION, RUN ANIM, EXCEPT WHEN AI SEE THE PLAYER VIA RAYCAST
                // THIS WILL GIVE A NEW ACTION WHICH IS SHOOT THE PLAYER
                if (distance > 2 && obstacle.Equals(false) && seen_by_AI.Equals(false))
                {
                    transform.Translate(Vector3.forward * speed);
                    transform.GetComponent<Rigidbody>().MovePosition(transform.position);
                    AnimateRun();
                }

                else
                {
                    #region DECREASE PLAYER SCORE
                    if (ScoreManager.instance.playerScore >= 1 && Time.time > nextFire)
                    {
                        ScoreManager.instance.playerScore--;
                        ScoreManager.instance.UpdateScore();
                        nextFire = Time.time + fireRate;
                    }
                    #endregion


                    #region SHAKE THE CAR
                    GameObject.Find("Car").GetComponent<Transform>().localEulerAngles = new Vector3(Random.Range(-5, 5), 180, Random.Range(-10, 10));
                    AnimateAttack();
                    #endregion

                    #region PLAYER LOSE OR IF PLAYER DOES NOT HAVE ANY SCORE BUT NEAR CONTACT WITH AI
                    if (PlayerController.instance != null &&
                       PlayerController.instance.earnedGems == true &&
                       ScoreManager.instance.playerScore == 0 ||
                       ScoreManager.instance.playerScore == 0 && PlayerController.instance.earnedGems == false)
                    {
                        //MAKE SURE IT REPEATED TO CALL
                        StartCoroutine(setEvent_GameOver(1));
                    }
                    #endregion
                }
            }
        }

        else
        {
            chasing = false;
        }
        #endregion
    }
    //-------------------------------------------------------------------------------------
    #endregion








    #region IENUMERATOR METHODS
    IEnumerator reset_Seen_By_AI(float value)
    {
        yield return new WaitForSeconds(value);
        seen_by_AI = false;
    }
    
    IEnumerator setEvent_GameOver(float value)
    {
        yield return new WaitForSeconds(value);
        PlayerController.instance.isPlayerLose = true;
        PlayerController.instance.smoke.SetActive(true);
        PlayerController.instance.smoke.GetComponent<Transform>().parent = null;
        PlayerController.instance.enabled = false;
        target = null;
    }
    
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

    public void AnimateCasting()
    {
        Animate(CASTING_BOOL);
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
