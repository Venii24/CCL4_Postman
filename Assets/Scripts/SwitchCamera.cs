using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class SwitchCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField] private float transitionDuration = 0.5f; // Duration of the transition
    [SerializeField] private Camera Camera2;
    [SerializeField] private float zoomFOV = 10f; // Target FOV for zooming in
    [SerializeField] private float normalFOV = 90f; // Normal FOV for zooming out
    [SerializeField] private Vector3 zoomInAngle = new Vector3(17f, 5f, 0f); // Angle when zoomed in
    [SerializeField] private Vector3 normalAngle = new Vector3(10f, 0f, 0f); // Normal camera angle

    private Vector3 targetRot;
    private Coroutine rotationCoroutine;
    private Coroutine zoomCoroutine;
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

    void OnZoomIn(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            ZoomIn();
        }
    }

    void OnZoomOut(InputValue inputValue)
    {
        if (inputValue.isPressed)
        {
            ZoomOut();
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

    public void ZoomIn()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCoroutine(zoomFOV, zoomInAngle));
    }

    public void ZoomOut()
    {
        if (zoomCoroutine != null)
        {
            StopCoroutine(zoomCoroutine);
        }
        zoomCoroutine = StartCoroutine(ZoomCoroutine(normalFOV, normalAngle));
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

    IEnumerator ZoomCoroutine(float targetFOV, Vector3 targetAngle)
    {
        SetAcceptInput(false);

        Camera currentCamera = Camera.main.enabled ? Camera.main : Camera2;
        float startFOV = currentCamera.fieldOfView;
        Quaternion startRotation = currentCamera.transform.rotation;
        Quaternion targetRotation = Quaternion.Euler(targetAngle);

        float elapsedTime = 0;

        while (elapsedTime < transitionDuration)
        {
            float t = elapsedTime / transitionDuration;
            currentCamera.fieldOfView = Mathf.Lerp(startFOV, targetFOV, t);
            currentCamera.transform.rotation = Quaternion.Lerp(startRotation, targetRotation, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }

        currentCamera.fieldOfView = targetFOV;
        currentCamera.transform.rotation = targetRotation;

        SetAcceptInput(true);
    }

    public void ResetRotation()
    {
        targetRot = Vector3.zero;
    }
}
