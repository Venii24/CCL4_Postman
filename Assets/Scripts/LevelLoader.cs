using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{
    public Animator transition;
    public float transitionTime = 2f;
    
    
    public void LoadNextLevel()
    {
        // StartCoroutine(LoadLevelAfterDelay(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void LoadLevelByName(string sceneName)
    {
        StartCoroutine(LoadLevelAfterDelay(sceneName));
    }

 

    IEnumerator LoadLevelAfterDelay(string sceneName)
    {
        transition.SetTrigger("Start");
        yield return new WaitForSeconds(transitionTime);
        SceneManager.LoadScene(sceneName);
    }
}