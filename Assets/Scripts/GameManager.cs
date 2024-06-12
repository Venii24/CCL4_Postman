using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public Timer timer;
    private int score = 0;
    private LevelLoader levelLoader;
    [SerializeField]
    public GameObject winOverlay;
    [SerializeField]
    public GameObject DialogueBox;
    [SerializeField]
    public GameObject Canvas;

    void Awake()
    {
        
        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            DontDestroyOnLoad(Canvas);
            levelLoader = FindObjectOfType<LevelLoader>(); // Find the LevelLoader in the scene
            
            // Disable the WinOverlay UI element at the start
            if (winOverlay != null)
                winOverlay.SetActive(false);
            
            if (DialogueBox != null)
                DialogueBox.SetActive(false);
        }
        else
        {
            Destroy(this.gameObject); // This will destroy the GameManager if another exists
        }
    }

    public void LoadScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
//Menu = 0 (By Built Index)
//Forest = 1
//Desert = 2
//Coast = 3
        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading next scene: " + nextSceneIndex);
            SceneManager.LoadScene(nextSceneIndex);
            winOverlay.SetActive(false);
            timer.CountdownTime = 120f;
        }
        else
        {
            Debug.Log("No more scenes to load.");
            // Optionally, you could reset to the first scene or show an end screen here
        }
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
    
    // Method to show the DialogueBox UI element
    public void ShowDialogueOverlay()
    {
        if (DialogueBox != null)
            DialogueBox.SetActive(true);
    }
    
    public void HideDialogueOverlay()
    {
        if (DialogueBox != null)
            DialogueBox.SetActive(false);
    }

    public void HideWinOverlay()
    {
        if (winOverlay != null)
            winOverlay.SetActive(false);
    }
}