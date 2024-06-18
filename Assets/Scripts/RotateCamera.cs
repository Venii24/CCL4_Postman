using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1;

    private Vector3 localRot;

    // Start is called before the first frame update
    void Start()
    {
        localRot = transform.rotation.eulerAngles; // Initialize localRot with current rotation
    }
    void Update()
    {
        PerformMovement();
        //transform.Rotate(0, speed * Time.deltaTime, 0);
    }
    
    void PerformMovement()
    {
        transform.Rotate(localRot * rotationSpeed);
    }

    void OnRotate(InputValue inputValue)
    {
        Vector2 rotation = inputValue.Get<Vector2>();
        localRot = new Vector3(0, rotation.x, 0);
    }
    
}