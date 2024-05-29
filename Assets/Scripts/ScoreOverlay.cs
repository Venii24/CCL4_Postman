using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreOverlay : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    // Start is called before the first frame update
    void Start()
    {
        scoreText = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        //Directly update the score text with the score from the GameManager
        if(scoreText != null && GameManager.Instance != null)
        {
            
            scoreText.text = $"Score: {GameManager.Instance.GetScore()}" ;
        }
    }
}
