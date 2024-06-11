using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rb;
    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction jumpAction;
    [SerializeField] float moveSpeed = 6f;
    [SerializeField] float jumpForce = 2f;
    [SerializeField] private Transform cam;
    [SerializeField] private float pushPower = 2.0f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions["Move"];
        jumpAction = playerInput.actions["Jump"];
    }

    void Update()
    {
        MovePlayer();
        Jump();
        RotatePlayer();
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

    // Function to push boxes
    void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Box"))
        {
            Rigidbody boxRigidbody = collision.collider.attachedRigidbody;
            if (boxRigidbody != null)
            {
                Vector3 forceDirection = collision.contacts[0].point - transform.position;
                forceDirection.y = 0;
                forceDirection.Normalize();

                boxRigidbody.AddForceAtPosition(forceDirection * pushPower, collision.contacts[0].point, ForceMode.Impulse);
            }
        }
    }
}