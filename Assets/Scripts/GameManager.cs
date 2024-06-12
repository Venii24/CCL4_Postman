using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int score = 0;
    private LevelLoader levelLoader;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject);
            levelLoader = FindObjectOfType<LevelLoader>(); // Find the LevelLoader in the scene
        }
        else
        {
            Destroy(this.gameObject); // This will destroy the GameManager if another exists
        }
    }

    public void LoadScene(string sceneName)
    {
        if (levelLoader != null)
        {
            levelLoader.LoadLevelByName(sceneName);
        }
        else
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
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

   
}