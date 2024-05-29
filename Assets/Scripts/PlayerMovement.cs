using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    PlayerInput _playerInput;
    InputAction _moveAction;
    [SerializeField] public float moveSpeed = 5f;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerInput = GetComponent<PlayerInput>();
        _moveAction = _playerInput.actions.FindAction("Move");
        
    }

    // Update is called once per frame
    void Update()
    {
     MovePlayer();
    }
    
    void MovePlayer()
    {
        Vector2 moveInput = _moveAction.ReadValue<Vector2>();
        Vector3 forward = transform.forward;
        Vector3 right = transform.right;
        Vector3 finalMoveDirection = (forward * moveInput.y + right * moveInput.x).normalized;
        transform.position += finalMoveDirection * Time.deltaTime * moveSpeed;

    }
}
