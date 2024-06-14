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
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
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