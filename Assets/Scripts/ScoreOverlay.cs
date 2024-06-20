using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }
    
    void Update()
    {
        if(scoreText != null && GameManager.Instance != null)
        {
            
            scoreText.text = $"Score: {GameManager.Instance.GetScore()}" ;
        }
    }
}
