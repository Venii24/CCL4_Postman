using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    
    [SerializeField] private float transitionDuration = 0.5f; // Duration of the transition
    private Vector3 targetRot;
    private Coroutine rotationCoroutine;

    void Start()
    {
        targetRot = transform.rotation.eulerAngles; // Initialize target rotation with current rotation
    }

    void Update()
    {
        PerformMovement();
    }

    void PerformMovement()
    {
        transform.rotation = Quaternion.Euler(targetRot);
    }

    void OnRotate(InputValue inputValue)
    {
        Vector2 rotation = inputValue.Get<Vector2>();

        if (rotation.x > 0) // Right arrow key
        {
            RotateCamera(90);
        }
        else if (rotation.x < 0) // Left arrow key
        {
            RotateCamera(-90);
        }
    }

    void RotateCamera(float angle)
    {
        // Calculate the target rotation
        targetRot.y += angle;

        // If there's a previous rotation coroutine, stop it
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }

        // Start a new coroutine for smooth rotation
        rotationCoroutine = StartCoroutine(RotateCoroutine(targetRot));
    }

    IEnumerator RotateCoroutine(Vector3 targetRotation)
    {
        Quaternion startRotation = transform.rotation;
        Quaternion targetRotationQuat = Quaternion.Euler(targetRotation);

        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            // Calculate the current rotation based on the interpolation
            float t = elapsedTime / transitionDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotationQuat, t);

            // Increment time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure final rotation is exactly the target rotation
        transform.rotation = targetRotationQuat;
    }
}