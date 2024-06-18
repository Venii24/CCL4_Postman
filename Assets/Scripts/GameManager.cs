using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Overlays")]
    [SerializeField] public GameObject winOverlay;
    [SerializeField] public GameObject WinWinOverlay;
    [SerializeField] public GameObject TimerBox;
    [SerializeField] public GameObject TimeOverOverlay;
    [SerializeField] public GameObject FKeyAlert;
    [SerializeField] public GameObject Instructions;

    [Header("Dont Destroy On Load")]
    [SerializeField] public GameObject DialogueManager;
    [SerializeField] public GameObject Canvas;
    [SerializeField] public GameObject LevelLoader;

    [Header("UI Elements")]
    [SerializeField] public TextMeshProUGUI ButtonLevelContinueText;
    [SerializeField] public RawImage StampsImage;
    [SerializeField] public TextMeshProUGUI PlayerTimeText;
    [SerializeField] public TextMeshProUGUI TotalStampsText;
    [SerializeField] public TextMeshProUGUI TotalTimeText;
    

    [Header("Stamp Images")]
    [SerializeField] private List<Texture> ForestStamps;
    [SerializeField] private List<Texture> DesertStamps;
    [SerializeField] private List<Texture> CoastStamps;

    public Animator transition;
    public static GameManager Instance { get; private set; }
    private Timer timer;
    public int score = 0;
    private bool backToMenu = false;
    private bool showWinWinOverlay = false;

    private int ForestScore = 0;
    private int DesertScore = 0;
    private int CoastScore = 0;
    private int TotalScore = 0;
    private int TimeForest = 0;
    private int TimeDesert = 0;
    private int TimeCoast = 0;
    private int TotalTime = 0;

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
            WinWinOverlay.SetActive(false);
            //Reset all Values to 0
                score = 0;  
                ForestScore = 0;
                DesertScore = 0;
                CoastScore = 0;
                TotalScore = 0;
                TimeForest = 0;
                TimeDesert = 0;
                TimeCoast = 0;
                TotalTime = 0;
            
        }
        else if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            StartCoroutine(ShowTimerWithDelay());
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            timer.stopTimer = false;
        }

        if (SceneManager.GetActiveScene().buildIndex == 1 || SceneManager.GetActiveScene().buildIndex == 2 || SceneManager.GetActiveScene().buildIndex == 3)
        {
            SetText();
            SetCollectableImage();
        }
    }

    public void LoadScene()
    {
        SaveLevelStats();
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;
        if (showWinWinOverlay == true)
        {
            WinWinOverlay.SetActive(true);
            showWinWinOverlay = false;
        }

        else
        {
            score = 0;
            // Menu = 0 (By Built Index)
            // Forest = 1
            // Desert = 2
            // Coast = 3

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
            {
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
                ButtonLevelContinueText.text = "Show Results";
                showWinWinOverlay = true;
            }

            if (nextSceneIndex == 1)
            {
                timer.CountdownTime = 183f;
            }

            
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
        SetCollectableImage(); // Update the image whenever the score changes
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
        score = 0;
        winOverlay.SetActive(false);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        timer.CountdownTime = 181f;
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

    public void SetText()
    {
        int timeNeeded = 180 - (int)timer.CountdownTime;
        //format to 00:00
        int minutes = timeNeeded / 60;
        int seconds = timeNeeded % 60;
        PlayerTimeText.text = $"{minutes:00}:{seconds:00}";
        
        int totalMinutes = TotalTime / 60;
        int totalSeconds = TotalTime % 60;
        
        TotalStampsText.text = $"You collected {TotalScore} out of 9 stamps!";
        TotalTimeText.text = $"{totalMinutes:00}:{totalSeconds:00}";
    }

    public void SetCollectableImage()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        List<Texture> stampList = null;

        switch (currentSceneIndex)
        {
            case 1:
                stampList = ForestStamps;
                break;
            case 2:
                stampList = DesertStamps;
                break;
            case 3:
                stampList = CoastStamps;
                break;
            default:
                Debug.LogWarning("Scene index not recognized for collectable images.");
                return;
        }

        if (stampList != null && score < stampList.Count)
        {
            StampsImage.texture = stampList[score];
            //Debug.Log($"Stamp image updated: Scene: {currentSceneIndex}, Score: {score}");
        }
        else
        {
            Debug.LogWarning($"Stamp image not found for Scene: {currentSceneIndex}, Score: {score}");
        }
    }

    private void SaveLevelStats()
    {
        int timeNeeded = 180 - (int)timer.CountdownTime;

        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (currentSceneIndex == 0)
        {
            return;
        }
        switch (currentSceneIndex)
        {
            case 1:
                ForestScore = score;
                TimeForest = timeNeeded;
                break;
            case 2:
                DesertScore = score;
                TimeDesert = timeNeeded;
                break;
            case 3:
                CoastScore = score;
                TimeCoast = timeNeeded;
                break;
        }

        TotalScore = ForestScore + DesertScore + CoastScore;
        TotalTime = TimeForest + TimeDesert + TimeCoast;

        Debug.Log($"Level {currentSceneIndex} stats saved: Score = {score}, Time = {timeNeeded}");
    }
}
