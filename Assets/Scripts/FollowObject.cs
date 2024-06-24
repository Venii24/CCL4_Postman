using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowObject : MonoBehaviour
{
    [SerializeField] private GameObject objectToFollow;
    // Start is called before the first frame update

    private Transform otherObjectTransform;
    private Transform ownTransform;
    private Vector3 offsetOfFollowObjectAndOwnObject;
    private float fixYOffset;
    private float distance;
    void Start()
    {
        otherObjectTransform = objectToFollow.GetComponent<Transform>();
        ownTransform = gameObject.GetComponent<Transform>();
        fixYOffset = ownTransform.position.y - otherObjectTransform.position.y;
        distance = Vector3.Distance(ownTransform.position, otherObjectTransform.position);
        SetOffSet();
    }

    // Update is called once per frame
    void Update()
    {
        gameObject.transform.position = otherObjectTransform.position + offsetOfFollowObjectAndOwnObject;
        offsetOfFollowObjectAndOwnObject.Normalize();
        offsetOfFollowObjectAndOwnObject *= distance;
        offsetOfFollowObjectAndOwnObject.y = fixYOffset;
    }
    public void SetOffSet()
    {
        offsetOfFollowObjectAndOwnObject = ownTransform.position - otherObjectTransform.position;

    }
}
