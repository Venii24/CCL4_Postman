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
    [SerializeField] public GameObject Instructions;

    [Header("Dont Destroy On Load")]
    [SerializeField] public GameObject DialogueManager;
    [SerializeField] public GameObject Canvas;
    [SerializeField] public GameObject LevelLoader;

    [Header("Button")]
    [SerializeField] public TextMeshProUGUI ButtonLevelContinueText;
    
    public Animator transition;
    public static GameManager Instance { get; private set; }
    private Timer timer;
    private int score = 0;
    private bool backToMenu = false;

    private void Awake()
    {
        timer = FindObjectOfType<Timer>();
  
        timer.stopTimer = true;

        if (Instance == null)
        {
            Instance = this;
            if (!backToMenu)
            {
                DontDestroyOnLoad(this.gameObject);
                DontDestroyOnLoad(Canvas);
                DontDestroyOnLoad(DialogueManager);
                DontDestroyOnLoad(LevelLoader);

                // Disable the WinOverlay UI element at the start
                if (winOverlay != null)
                    winOverlay.SetActive(false);

                if (TimeOverOverlay != null)
                    TimeOverOverlay.SetActive(false);
                
                if (Instructions != null)
                    Instructions.SetActive(false);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }
        else
        {
            Destroy(this.gameObject);
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
            StartCoroutine(ShowTimerWithDelay());
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            timer.stopTimer = false;
        }
        
     
    }

    public void LoadScene()
    {
        score = 0;
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        // Menu = 0 (By Built Index)
        // Forest = 1
        // Desert = 2
        // Coast = 3

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            Debug.Log("Loading next scene: " + nextSceneIndex);
            StartCoroutine(LoadLevel(nextSceneIndex));
            winOverlay.SetActive(false);
            timer.CountdownTime = 181f;
            TimerBox.SetActive(true);
            timer.stopTimer = false;
        }
        else if (nextSceneIndex == 4) // Go back to menu
        {
            SceneManager.LoadScene(0);
            DestroyPersistentObjects();
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
        if (nextSceneIndex == 1)
        {
            timer.CountdownTime = 183f;
        }
    }
    
    IEnumerator LoadLevel(int levelIndex)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(2f);
        SceneManager.LoadScene(levelIndex);
    }

    private void DestroyPersistentObjects()
    {
        // Destroy GameManager instance
        Destroy(this.gameObject);
        
        // Destroy other objects set to DontDestroyOnLoad
        Destroy(Canvas);
        Destroy(DialogueManager);
        Destroy(LevelLoader);

        // Reset static instance
        Instance = null;
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
    

    public void OnWinOverlayButtonClicked()
    {
        LoadScene();
    }
    
    public void ShowInstructions()
    {
        if (Instructions != null)
            Instructions.SetActive(true);
    }
    
    public void HideInstructions()
    {
        if (Instructions != null)
            Instructions.SetActive(false);
    }
    
    IEnumerator ShowTimerWithDelay()
    {
        while (true)
        {
            yield return new WaitForSeconds(2f);
            TimerBox.SetActive(true);
        }
    }
}
