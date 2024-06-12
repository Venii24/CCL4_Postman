using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int score = 0;
    private LevelLoader levelLoader;
    [SerializeField]
    public GameObject winOverlay;
    [SerializeField]
    public GameObject DialogueBox;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            levelLoader = FindObjectOfType<LevelLoader>(); // Find the LevelLoader in the scene
            
            // Disable the WinOverlay UI element at the start
            if(winOverlay != null)
                winOverlay.SetActive(false);
            
            if(DialogueBox != null)
                DialogueBox.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject); // This will destroy the GameManager if another exists
        }
    }

    public void LoadScene(string sceneName)
    {
        Debug.Log("Loading scene: " + sceneName);
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    public void AddScore(int scoreToAdd)
    {
        score += scoreToAdd;
    }

    public int GetScore()
    {
        return score;
    }

    // Method to show the WinOverlay UI element
    public void ShowWinOverlay()
    {
        if (winOverlay != null)
            winOverlay.SetActive(true);
    }
    
    // Method to show the WinOverlay UI element
    public void ShowDialogueOverlay()
    {
        if (DialogueBox != null)
            DialogueBox.SetActive(true);
    }
    
    public void HideDialogueOverlay()
    {
            DialogueBox.SetActive(false);
            winOverlay.SetActive(false);
    }
}