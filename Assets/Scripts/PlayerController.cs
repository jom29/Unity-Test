using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class PlayerController : MonoBehaviour
{

    public Material defaultSkin;
    public PlayerInput playerInput;
    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    public float playerSpeed = 2.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public GameObject[] gems;


    void Start()
    {
        controller = gameObject.AddComponent<CharacterController>();
        gems = GameObject.FindGameObjectsWithTag("gems");
        defaultSkin.color = Color.grey;
    }


    private void Update()
    {
        MovementController();
    }






    void MovementController()
    {

        Vector2 left_stick = playerInput.actions["MoveAction"].ReadValue<Vector2>();



        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        // Vector3 move = new Vector3(left_stick.x , 0, left_stick.y);
        Vector3 move = new Vector3(left_stick.x + Input.GetAxis("Horizontal"), 0, left_stick.y + Input.GetAxis("Vertical"));




        controller.Move(move * Time.deltaTime * playerSpeed);

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;
        }

        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
