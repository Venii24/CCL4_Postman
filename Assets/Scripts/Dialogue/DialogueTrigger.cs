using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueTrigger : MonoBehaviour
{
    [Header("Ink JSON File")]
    [SerializeField] private TextAsset inkJSON;
    [SerializeField] public Texture characterImagePNG;
    
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
            DialogueManager.GetInstance().EnterDialogueMode(inkJSON, characterImagePNG);
        }
        else if (playerInRange && DialogueManager.GetInstance().dialogueIsPlaying) // If player is in range but dialogue is playing
        {
            DialogueManager.GetInstance().FKeyAlert.SetActive(false);
        }
        else if (playerInRange && !DialogueManager.GetInstance().dialogueIsPlaying) // If player is in range but dialogue is not playing
        {
            DialogueManager.GetInstance().FKeyAlert.SetActive(true);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = true;
        }
        DialogueManager.GetInstance().FKeyAlert.SetActive(false);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerInRange = false;
        }
        DialogueManager.GetInstance().FKeyAlert.SetActive(false);
    }
}