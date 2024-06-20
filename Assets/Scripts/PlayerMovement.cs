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
    [SerializeField] private GameObject Stamp1;
    [SerializeField] private GameObject Stamp2;
    [SerializeField] private GameObject Stamp3;
    [SerializeField] private Transform cam;
    [SerializeField] private GameObject Mark;
    [SerializeField] private GameObject MarkTrigger;
    
    
    
    [Header("Variables")] 
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] private float pushPower = 2.0f;

    [SerializeField]
    private Animator _animator;

    [SerializeField] private float jumpAnimationDuration = 0.5f; // Duration of the jump animation
        
    private GameManager gameManager; 
    private Timer timer;
    private Rigidbody rb;
    private SwitchCamera camRotator;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    private Vector3 box1StartPos;
    private Vector3 box2StartPos;
    private Vector3 stamp1StartPos;
    private Vector3 stamp2StartPos;
    private Vector3 stamp3StartPos;
    private float originalSpeed;

    public bool letterDelivered = false;
    public bool inNPCArea = false;
    public bool MarkStep2 = false;
    
    public string currentSurfaceType = null; // Default surface type

    void Start()
    {
        MarkTrigger.SetActive(false);
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        camRotator = FindObjectOfType<SwitchCamera>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
        letterDelivered = false;
        box1StartPos = box1.transform.position;
        box2StartPos = box2.transform.position;
        stamp1StartPos = Stamp1.transform.position;
        stamp2StartPos = Stamp2.transform.position;
        stamp3StartPos = Stamp3.transform.position;
        originalSpeed = moveSpeed;

        // Find the GameManager instance in the scene
        gameManager = GameManager.Instance;
        timer = FindObjectOfType<Timer>();
    }

    void Update()
    {
     
        if (letterDelivered && MarkStep2) Mark.transform.position = new Vector3(0.13f, 3.95f, -18f);
        else if (letterDelivered)
        {
            Mark.transform.position = new Vector3(0.29f, 1.678f, -9.16f);
            MarkTrigger.SetActive(true);
        }
        
        
        if (DialogueManager.GetInstance().dialogueIsPlaying)
        {
            return;
        }
       // Debug.Log(letterDelivered);
        
        DetectSurface();
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
    
    public void HideMark()
    {
        Mark.SetActive(false);
    }

    void DetectSurface()
    {
        // Cast a ray downwards to detect the surface type
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1f))
        {
            string surfaceTag = hit.collider.tag;

            switch (surfaceTag)
            {
                case "Grass":
                    currentSurfaceType = "grass";
                    break;
                case "Sand":
                    currentSurfaceType = "sand";
                    break;
                case "Wood":
                    currentSurfaceType = "wood";
                    break;
                case "Stone":
                    currentSurfaceType = "stone";
                    break;
               
                default:
                    currentSurfaceType = null; // Handle unknown tags
                    break;
            }
        }
        else
        {
            currentSurfaceType = null; // Handle case when no surface is detected
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

        // Ensure player faces the direction of movement
        if (isWalking)
        {
            RotatePlayer();
        }
    }
    
    void RotatePlayer()
    {
        Vector2 moveInput = moveAction.ReadValue<Vector2>();
        if (moveInput.magnitude > 0.1f)
        {
            float targetAngle = Mathf.Atan2(moveInput.x, moveInput.y) * Mathf.Rad2Deg + cam.eulerAngles.y;
            //targetAngle += 0; // Rotate 90° to the right - had to change it with the new import
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, angle, 0f);
        }
    }

    private float turnSmoothVelocity;
    public float turnSmoothTime = 0.1f;
    
    void Jump()
    {
        if (jumpAction.triggered && Mathf.Abs(rb.velocity.y) < 0.01f && Physics.Raycast(transform.position, Vector3.down, 1.1f))
        {
            _animator.SetBool("isJumping", true); 
            rb.velocity = new Vector3(rb.velocity.x, jumpForce, rb.velocity.z);
            StartCoroutine(ResetJumpAnimation());
        }
    }

    IEnumerator ResetJumpAnimation()
    {
        yield return new WaitForSeconds(jumpAnimationDuration);
        _animator.SetBool("isJumping", false);
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

                // Check for player input
                bool isPushing = Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0;

                if (isPushing)
                {
                    _animator.SetBool("isPushing", true);
                    moveSpeed = originalSpeed / 1.5f;
                    boxRigidbody.AddForce(forceDirection * pushPower, ForceMode.Impulse);
                }
                else
                {
                    moveSpeed = originalSpeed;
                    _animator.SetBool("isPushing", false);
                }
            }
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.collider.CompareTag("Box"))
        {
            moveSpeed = originalSpeed;
            _animator.SetBool("isPushing", false);
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("NPCArea"))
        {
            inNPCArea = true;
        }
        
        if (other.CompareTag("Mark"))
        {
            MarkStep2 = true;
        }
      
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("NPCArea"))
        {
            inNPCArea = false;
            //Debug.Log("Exited NPC area: " + other.name);
        }
    }
    

    private void PlayerDying()
    {
        if (transform.position.y < -10)
        {
            letterDelivered = false;
            gameManager.score = 0;
            Die();
           
        }
    }

    private void Die()
    {
        Debug.Log("Player died!");
        //set player to start position
        transform.position =  new Vector3(0, 1.7f, -9f);
        //reset box positions
        box1.transform.position = box1StartPos;
        box2.transform.position = box2StartPos;
        //reset stamp positions
        Stamp1.SetActive(true);
        Stamp2.SetActive(true);
        Stamp3.SetActive(true);
        Stamp1.transform.position = stamp1StartPos;
        Stamp2.transform.position = stamp2StartPos;
        Stamp3.transform.position = stamp3StartPos;
        //reset score
        gameManager.score = 0;
        camRotator.ResetRotation();
    }
}
