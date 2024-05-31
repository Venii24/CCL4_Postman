using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RotateCamera : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 1;
    //[SerializeField] private float orbitDamping = 10;
    //[SerializeField] private float scrollRotationSpeed = 10; // Added field for mouse wheel rotation speed

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
    
    
    // Camera rotation with mouse
    // void Update()
    // {
    //     localRot.x += Input.GetAxis("Mouse X") * rotationSpeed;
    //     localRot.y -= Input.GetAxis("Mouse Y") * rotationSpeed;
    //
    //     // Clamp the y rotation to prevent flipping
    //     localRot.y = Mathf.Clamp(localRot.y, 0f, 0f);
    //
    //     // Rotate around y-axis using mouse wheel
    //     localRot.x += Input.GetAxis("Mouse ScrollWheel") * scrollRotationSpeed;
    //
    //     Quaternion QT = Quaternion.Euler(localRot.y, localRot.x, 0f);
    //     transform.rotation = Quaternion.Lerp(transform.rotation, QT, Time.deltaTime * orbitDamping);
    // }
}