using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    [SerializeField] private float transitionDuration = 0.5f; // Duration of the transition
    [SerializeField] private Camera Camera2;
    private Vector3 targetRot;
    private Coroutine rotationCoroutine;
    private bool acceptInput = true;

    void Start()
    {
        targetRot = transform.rotation.eulerAngles; 
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
        if (!acceptInput) return;

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
        targetRot.y += angle;
        
        if (rotationCoroutine != null)
        {
            StopCoroutine(rotationCoroutine);
        }
        
        rotationCoroutine = StartCoroutine(RotateCoroutine(targetRot));
    }

    public void ChangeToCamera2()
    {
        Camera.main.enabled = false;
        Camera2.enabled = true;
    }
    
    public void ChangeToMainCamera()
    {
        Camera.main.enabled = true;
        Camera2.enabled = false;
    }

    public void SetAcceptInput(bool accept)
    {
        acceptInput = accept;
    }

    IEnumerator RotateCoroutine(Vector3 targetRotation)
    {
        SetAcceptInput(false);

        Quaternion startRotation = transform.rotation;
        Quaternion targetRotationQuat = Quaternion.Euler(targetRotation);

        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            transform.rotation = Quaternion.Lerp(startRotation, targetRotationQuat, t);
            
            elapsedTime += Time.deltaTime;

            yield return null;
        }

       
        transform.rotation = targetRotationQuat;

       
        SetAcceptInput(true);
    }
    
    
    public void ResetRotation()
    {
        targetRot = Vector3.zero;
    }
}