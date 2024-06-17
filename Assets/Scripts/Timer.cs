using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Timer : MonoBehaviour
{
    public GameManager gameManager;
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private RawImage timerImage;
    public float CountdownTime = 182f;
    public bool stopTimer = false;

    private Coroutine blinkCoroutine;
    private DialogueManager dialogueManager;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        dialogueManager = FindObjectOfType<DialogueManager>();
    }

    // Update is called once per frame
    void Update()
    { 
        if (dialogueManager.dialogueIsPlaying)
        {
            stopTimer = true;
        }
        
        if (!stopTimer)
        {
            CountdownTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(CountdownTime / 60);
            int seconds = Mathf.FloorToInt(CountdownTime % 60);
            timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
        }
        
       

        if (CountdownTime <= 0)
        {
            gameManager.ShowTimeOverOverlay();
            timerText.text = "00:00";
            stopTimer = true;
        }
        else
        {
            gameManager.HideTimeOverOverlay();
        }

        HandleTimerBlink();
    }

    private void HandleTimerBlink()
    {
        if (stopTimer && blinkCoroutine == null)
        {
            blinkCoroutine = StartCoroutine(BlinkTextAndImage());
            stopTimer = true;
        }
        else if (!stopTimer && blinkCoroutine != null)
        {
            StopCoroutine(blinkCoroutine);
            blinkCoroutine = null;
            timerText.enabled = true; // Ensure the text is visible when not blinking
            timerImage.enabled = true; // Ensure the image is visible when not blinking
        }
    }

    private IEnumerator BlinkTextAndImage()
    {
        while (true)
        {
            timerText.enabled = !timerText.enabled;
            timerImage.enabled = !timerImage.enabled;
            yield return new WaitForSeconds(0.5f); // Adjust blink speed here
        }
    }
}