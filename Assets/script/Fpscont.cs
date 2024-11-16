using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fpscont : MonoBehaviour
{
    CharacterController controller;

    Vector3 velocity;
    bool isGrounded;

    public Transform ground;
    public float distance = 0.3f;

    public float speed;
    public float jumpHeight;

    public float gravity; 

    public LayerMask mask;

    public bool canMove;

    public float orginalHeight;
    public float crouchHeight;
    void Start()
    {
        controller = GetComponent<CharacterController>();
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        if (canMove)
        {
            controller.Move(move * speed * Time.deltaTime);
        }

        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            gravity = -25f;
            velocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravity);
        }
        
        isGrounded = Physics.CheckSphere(ground.position, distance, mask);
        
        if (isGrounded && velocity.y < 0)
        {
            velocity.y = 0f;
            
        }



        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);


        if (Input.GetKeyDown(KeyCode.LeftControl))
        {
            controller.height = crouchHeight;
            speed = 2f;
            jumpHeight = 0.5f;
            gravity = -500f;
        }

        if (Input.GetKeyUp(KeyCode.LeftControl))
        {
            gravity = 0;
            controller.height = orginalHeight;
            speed = 5f;
            jumpHeight = 2f;
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {

            if (controller.height == 1)
            {
                speed += 5f;
            }
        }

        if (Input.GetKeyUp(KeyCode.LeftShift))
        {
          
            if (controller.height == 1)
            {
                speed -= 5f;
            }
        }
    }
}
