using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI timerText;
    public float CountdownTime = 180f;
    public bool stopTimer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (!stopTimer)
        {
            CountdownTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(CountdownTime / 60);
            int seconds = Mathf.FloorToInt(CountdownTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds); 
        }
       
    }
}
