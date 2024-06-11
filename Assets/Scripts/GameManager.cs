using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
 
class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    private int _score = 0;
    private LevelLoader levelLoader;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this.gameObject); // This will make the GameManager persist between scenes
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

    public void AddScore(int score)
    {
        _score += score;
    }

    public int GetScore()
    {
        return _score;
    }

    void Start()
    {
        
    }

    void Update()
    {
        //  Debug.Log("Score: " + _score);
    }
}