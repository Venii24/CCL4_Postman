using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    [SerializeField]
    private GameObject playerFigure;
    private FollowObject _followObject;
    // Start is called before the first frame update
    void Start()
    {
        _followObject = gameObject.GetComponent<FollowObject>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCameraRotation(InputValue inputValue)
    {

        Vector2 input = inputValue.Get<Vector2>();
        transform.RotateAround(playerFigure.transform.position, new Vector3(0, 1, 0), input.x);
        _followObject.SetOffSet();


    }
}
