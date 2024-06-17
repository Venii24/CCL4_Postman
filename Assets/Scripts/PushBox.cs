using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBox : MonoBehaviour
{
    [SerializeField]
    //private float pushPower = 2.0f;

    private void OnCollisionEnter(Collision collision)
    {
        Rigidbody rb = collision.collider.attachedRigidbody;
        if (rb == null || rb.isKinematic)
        {
            return;
        }

        
       
    }
    
}