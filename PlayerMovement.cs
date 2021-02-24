using System.Collections;
using UnityEngine;

namespace Movement
{
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerMovement : MonoBehaviour
    {
        // This Script was created by following this tutorial: https://www.youtube.com/watch?v=NEUzB5vPYrE&list=LL&index=2&t=1175s
        // I've added comments where I can to explain my understanding and have done my best to use the advice presented and create my own version rather than copy.

        [Header("Player Movement Configuration")]
        public float walkSpeed;
        public float runSpeed;
        public bool isDead;

        public float playerJumpForce;
        public ForceMode appliedForceMode;

        public Transform groundCheck;
        public float groundDistance = 0.4f;
        public LayerMask groundMask;


        public bool playerIsJumping;
        public bool isGrounded;
        public float currentSpeed;


        private float m_xAxis;
        private float m_zAxis;
        private Rigidbody player_rb;

        private Animator anim;

        private void Start()
        {
            // As specified in the Coursework description, our character needs to be using a Rigidbody so that it interacts with physics objects.
            player_rb = GetComponent<Rigidbody>();

            // I've created a 'ground' layer in my Level to exclusively for detecting collisions.
            groundMask = LayerMask.GetMask("ground");

            // Attaching Animation controller
            anim = GetComponent<Animator>();

        }

        private void Update()
        {
            
            anim.SetBool("Attack", false);
            // Getting input via Unity's old input system; Controls are configured in Edit -> Project Settings -> Input Manager
            m_xAxis = Input.GetAxis("Horizontal");
            m_zAxis = Input.GetAxis("Vertical");

            // Check to see if Player is pressing left shift (i.e. they want the character to run) 
            currentSpeed = Input.GetKey(KeyCode.LeftShift) ? runSpeed : walkSpeed;

            if (currentSpeed == walkSpeed)
            {
                anim.SetFloat("Speed", 0.5f, 0.1f, Time.deltaTime);
            }
            else if (currentSpeed == runSpeed)
            {
                anim.SetFloat("Speed", 1.0f, 0.1f, Time.deltaTime);
            }

            if (Input.GetKey(KeyCode.Mouse0))
            {
                anim.SetBool("Attack", true);
            }

            isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask) ? true : false;

            // Check to see if Space is being pressed; if so it means the player is wanting to Jump.
            playerIsJumping = Input.GetKey(KeyCode.Space) ? true : false;
            //Debug.Log("playerIsJumping? " + playerIsJumping);
        }

        private void FixedUpdate()
        {

            player_rb.MovePosition(transform.position + Time.deltaTime * currentSpeed *
                transform.TransformDirection(m_xAxis, 0f, m_zAxis));

            // Ensuring that a) Player isn't currently mid-jump and b) they're not currently falling or above the ground.
            if (playerIsJumping && isGrounded)
            {
                PlayerJump(playerJumpForce, appliedForceMode);
            }

        }
        private void PlayerJump(float jumpForce, ForceMode forceMode)
        {
            // Apply an upwards force to the player's rigidbody component
            player_rb.AddForce(jumpForce * player_rb.mass * Time.deltaTime * Vector3.up, forceMode);
        }
    }
}
