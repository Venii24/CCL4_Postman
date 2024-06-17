using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class PlayerMovement : MonoBehaviour
{

    [Header("Game Objects")] 
    [SerializeField]
    private GameObject box1;
    [SerializeField]
    private GameObject box2;
    [SerializeField] private Transform cam;
    
    
    [Header("Variables")] 
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] private float pushPower = 2.0f;

    [SerializeField]
    private Animator _animator;
        
    private GameManager gameManager; 
    private Timer timer;
    private Rigidbody rb;
    private SwitchCamera camRotator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector3 box1StartPos;
    private Vector3 box2StartPos;
 
    public bool letterDelivered = false;
    public bool inNPCArea = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        camRotator = FindObjectOfType<SwitchCamera>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        letterDelivered = false;
        box1StartPos = box1.transform.position;
        box2StartPos = box2.transform.position;
        
        // Find the GameManager instance in the scene
        gameManager = GameManager.Instance;
        timer = FindObjectOfType<Timer>();
    }

    void Update()
    {
        /*
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        */
        
        MovePlayer();
        Jump();
        RotatePlayer();
        PlayerDying();
        // Check for letter delivery when in NPC area
        // DeliverLetter();
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void MovePlayer()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        
        // Camera direction
        Vector3 camForward = cam.forward;
        Vector3 camRight = cam.right;
        
        camForward.y = 0;
        camRight.y = 0;
        
        // Create camera-relative directions
        Vector3 forwardRelative = camForward.normalized * moveInput.y;
        Vector3 rightRelative = camRight.normalized * moveInput.x;
        
        Vector3 moveDirection = forwardRelative + rightRelative;
        
        // Apply movement
        rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, moveDirection.z * moveSpeed);

        // Update animator parameter
        bool isWalking = moveDirection.magnitude > 0.1f;
        _animator.SetBool("isWalking", isWalking);
    }

    void Jump()
    {
        if (jumpAction.triggered && Mathf.Abs(rb.velocity.y) < 0.01f && Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
        }
    }

    void RotatePlayer()
    {
        Vector3 direction = new Vector3(rb.velocity.x, 0, rb.velocity.z);
        if (direction.magnitude > 0.1f)
        {
            transform.forward = direction;
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Box"))
        {
            Rigidbody boxRigidbody = collision.collider.attachedRigidbody;
            if (boxRigidbody != null)
            {
                Vector3 forceDirection = (collision.transform.position - transform.position).normalized;
                forceDirection.y = 0; // Ignore the y-axis

                if (Mathf.Abs(forceDirection.x) > Mathf.Abs(forceDirection.z))
                {
                    forceDirection = new Vector3(Mathf.Sign(forceDirection.x), 0, 0);
                }
                else
                {
                    forceDirection = new Vector3(0, 0, Mathf.Sign(forceDirection.z));
                }

                boxRigidbody.AddForce(forceDirection * pushPower, ForceMode.Impulse);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPCArea"))
        {
            inNPCArea = true;
            Debug.Log("Entered NPC area: " + other.name);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPCArea"))
        {
            inNPCArea = false;
            Debug.Log("Exited NPC area: " + other.name);
        }
    }
    

    private void PlayerDying()
    {
        if (transform.position.y < -10)
        {
            letterDelivered = false;
            Die();
           
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        //set player to start position
        transform.position =  new Vector3(0, 1.7f, -14f);
        //reset box positions
        box1.transform.position = box1StartPos;
        box2.transform.position = box2StartPos;
        camRotator.ResetRotation();
    }
}
