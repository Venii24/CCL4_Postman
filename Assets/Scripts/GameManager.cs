using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    
    [Header("Overlays")]
    [SerializeField] public GameObject winOverlay;
    [SerializeField] public GameObject TimerBox;
    [SerializeField] public GameObject TimeOverOverlay;
    [SerializeField] public GameObject FKeyAlert;
    
     [Header("Dont Destroy On Load")]
     [SerializeField] public GameObject DialogueManager;
     [SerializeField] public GameObject Canvas;
     
     [Header("Buttons")]
     [SerializeField] public TextMeshProUGUI ButtonLevelContinueText;
    public static GameManager Instance { get; private set; }
    private Timer timer;
    private int score = 0;
    private LevelLoader levelLoader;
    private bool BacktoMenu = false;
   
   
   

    

    void Awake()
    {
        timer = FindObjectOfType<Timer>();
        timer.stopTimer = true;

        if (Instance == null)
        {
            Instance = this;
            if (!BacktoMenu)
            {
                DontDestroyOnLoad(this.gameObject);
                DontDestroyOnLoad(Canvas);
                DontDestroyOnLoad(DialogueManager);
                levelLoader = FindObjectOfType<LevelLoader>(); // Find the LevelLoader in the scene

                // Disable the WinOverlay UI element at the start
                if (winOverlay != null)
                    winOverlay.SetActive(false);

                if (TimeOverOverlay != null)
                    TimeOverOverlay.SetActive(false);
            }
            else
            {
                Destroy(this.gameObject); // This will destroy the GameManager if another exists
            }
            

        }


    }

    private void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            winOverlay.SetActive(false);
            TimeOverOverlay.SetActive(false);
            FKeyAlert.SetActive(false);
            TimerBox.SetActive(false);
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            TimerBox.SetActive(true);
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
            timer.CountdownTime = 180f;
            TimerBox.SetActive(true);
            timer.stopTimer = false;
        }
        else if (nextSceneIndex == 4) //go back to menu
        {
            SceneManager.LoadScene(0);
            winOverlay.SetActive(false);
            TimeOverOverlay.SetActive(false);
            TimerBox.SetActive(false);
            timer.CountdownTime = 180f;
            timer.stopTimer = true;
            
        }
        else
        {
            Debug.Log("No more scenes to load.");
            // Optionally, you could reset to the first scene or show an end screen here
        }
        if (nextSceneIndex == 3)
        {
            ButtonLevelContinueText.text = "Back to Menu";
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
    
    public void ShowWinOverlay()
    {
        if (winOverlay != null)
            winOverlay.SetActive(true);
    }
    
    public void ShowTimeOverOverlay()
    {
        if (TimeOverOverlay != null)
            TimeOverOverlay.SetActive(true);
    }
    
    public void HideTimeOverOverlay()
    {
        if (TimeOverOverlay != null)
            TimeOverOverlay.SetActive(false);
    }
    
    public void reloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        timer.CountdownTime = 180f;
        timer.stopTimer = false;
    }

    public void HideWinOverlay()
    {
        if (winOverlay != null)
            winOverlay.SetActive(false);
    }
}