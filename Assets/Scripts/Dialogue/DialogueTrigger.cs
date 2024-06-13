using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    
    [Header("Ink JSON File")]
    [SerializeField] private TextAsset inkJSON;
    
    private bool playerInRange;
    void Start()
    {
        playerInRange = false;
        
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.Q))
        {
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
        
        
    }

    private void OnTriggerEnter(Collider other)
    { if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
