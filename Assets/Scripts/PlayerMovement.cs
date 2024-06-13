using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private GameManager gameManager; 
    private Timer timer;
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] private Transform cam;
    [SerializeField] private float pushPower = 2.0f;
    public bool letterDelivered = true;
    public bool inNPCArea = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];

        // Find the GameManager instance in the scene
        gameManager = GameManager.Instance;
        timer = FindObjectOfType<Timer>();
    }

    void Update()
    {
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
        
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

    // private void DeliverLetter()
    // {
    //     if (inNPCArea && !letterDelivered && Input.GetKeyDown(KeyCode.F))
    //     {
    //         Debug.Log("Letter delivered!");
    //         letterDelivered = true;
    //         playerInput.enabled = false;
    //         gameManager.ShowDialogueOverlay();
    //         timer.stopTimer = true;
    //     }
    //     else if (inNPCArea && letterDelivered && Input.GetKeyDown(KeyCode.E))
    //     {
    //         Debug.Log("Letter delivered!");
    //         playerInput.enabled =true ;
    //         gameManager.HideDialogueOverlay();
    //         timer.stopTimer = false;
    //     }
    //     
    // }

    private void PlayerDying()
    {
        if (transform.position.y < -10)
        {
            letterDelivered = false;
            Die();
            UnableOverlays();
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().name);
    }
    
    private void UnableOverlays()
    {
        gameManager.HideDialogueOverlay();
    }
}