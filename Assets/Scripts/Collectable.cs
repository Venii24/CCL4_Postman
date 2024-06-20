using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    
    // Rotation speed
    public float rotationSpeed = 70f;

    // Movement variables
    public float amplitude = 0.1f; 
    public float frequency = 1f; 

    private Vector3 startPos;
    
    void Start()
    {
        startPos = transform.position;
    }
    
    void Update()
    {
        // Rotate the collectable
        transform.Rotate(Vector3.down, rotationSpeed * Time.deltaTime);

        // Move the collectable up and down
        float newY = startPos.y + Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = new Vector3(transform.position.x, newY, transform.position.z);
    }
    
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            AkSoundEngine.PostEvent("Play_pickup_stamp", gameObject);
            GameManager.Instance.AddScore(1);
            gameObject.SetActive(false);
        }
    }
}