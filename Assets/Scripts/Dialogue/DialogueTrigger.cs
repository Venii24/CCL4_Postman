using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    
    [Header("Ink JSON File")]
    [SerializeField] private TextAsset inkJSON;
    private PlayerMovement playerMovement;
    
    private bool playerInRange;
    void Start()
    {
        playerInRange = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        
    }
    
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F) && !DialogueManager.GetInstance().dialogueIsPlaying)
        {
            playerMovement.letterDelivered = true;
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON);
        }
        // else if (playerInRange && Input.GetKeyDown(KeyCode.Space) && DialogueManager.GetInstance().dialogueIsPlaying)
        // {
        //     DialogueManager.GetInstance().ExitDialogueMode();
        // }
        
        
    }

    private void OnTriggerEnter(Collider other)
    { if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player in range");
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player out of range");
            playerInRange = false;
        }
    }
}
