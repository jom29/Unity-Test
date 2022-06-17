using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;
    public bool isPlayerLose;
    public bool earnedGems;
    public GameObject smoke;
    public Material defaultSkin;
    public PlayerInput playerInput;
    public Transform CarTransform;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public GameObject[] gems;
    private GameObject[] enemies;
    public bool someAI_chasingPlayer;

    [Header("Sound Rate")]
    public float soundRate = 0.5F;
    private float nextSound = 0.0F;



    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        enemies = GameObject.FindGameObjectsWithTag("enemy");
        controller = gameObject.AddComponent<CharacterController>();
        gems = GameObject.FindGameObjectsWithTag("gems");
        defaultSkin.color = Color.grey;


        if(CarTransform == null)
        {
            GameObject carGO = GameObject.Find("Car");
            CarTransform = carGO.GetComponent<Transform>();
        }
    }


    private void Update()
    {
        MovementController();
    }






    void MovementController()
    {
        //INPUTS -------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        Vector2 left_stick = playerInput.actions["MoveAction"].ReadValue<Vector2>();

        Vector3 move = new Vector3(left_stick.x + Input.GetAxis("Horizontal"), 0, left_stick.y + Input.GetAxis("Vertical"));
        //=================================================================================================================================================================================INPUTS (ENDLINE)



        //GRAVITY---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        groundedPlayer = controller.isGrounded;

        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }
        //==================================================================================================================================================================================GRAVITY (ENDLINE)





        //WHEN CAR IS MOVING ----------------------------------------------------------------------------------------------------------------------------------------------------------------------
        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;

            if (!SoundManager.instance.source[1].isPlaying)
            {
                SoundManager.instance.PlaySound(1);
            }
        }
        //==========================================================================================================================================================================WHEN CAR IS MOVING (ENDLINE)






        //WHEN CAR IS IN IDLE --------------------------------------------------------------------------------------------------------------------------------------------------------------------------
        if(move == Vector3.zero)
        {
            if (SoundManager.instance.source[1].isPlaying)
            {
                SoundManager.instance.Stop(1);
            }

            if(!SoundManager.instance.source[2].isPlaying)
            {
                SoundManager.instance.PlaySound(2);
            }
        }
        //==========================================================================================================================================================================WHEN CAR IS IN IDLE (ENDLINE)

        





        #region WHEN THE AI IS FAR AWAY FROM THE PLAYER  THEN RESET THE CAR LOCAL TRANSFORM
        foreach (GameObject enemy in enemies)
            {

               //CHECK WHEN AI SCRIPT IS NOT DESTROY
               if(enemy.GetComponent<AI>() != null)
               {
                   if (enemy != null && enemy.GetComponent<AI>().chasing == true)
                   {
                      float dist = Vector3.Distance(transform.position, enemy.GetComponent<Transform>().position);
                      if (dist > 5)
                      {
                        CarTransform.localEulerAngles = new Vector3(0, 180, 0);
                      }
                   }

                   if (enemy == null)
                   {
                    Destroy(enemy);
                   }
               }
               
            }
            #endregion
        

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
