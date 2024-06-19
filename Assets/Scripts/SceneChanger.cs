using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToEnter;
    [SerializeField] public GameObject train;
    [SerializeField] public GameObject trainObject;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject playerCamera;

    private Timer timer;
    private PlayerMovement playerMovement;
    private SwitchCamera switchCamera;
    private GameManager gameManager;
    private CollectableManager collectableManager;
    
    private Animator trainAnimator;

    private Vector3 cameraGameplayPosition = new Vector3(0, 13, -18f);
    private Vector3 cameraAnimationPosition = new Vector3(0, 13, -26);
    
    private void Start()
    {
        timer = FindObjectOfType<Timer>();
        GetComponent<MeshRenderer>().enabled = false;
        playerMovement = FindObjectOfType<PlayerMovement>();
        switchCamera = FindObjectOfType<SwitchCamera>();
        collectableManager = FindObjectOfType<CollectableManager>();
        gameManager = GameManager.Instance;
        timer.stopTimer = true;
        switchCamera.ChangeToMainCamera();

        trainAnimator = trainObject.GetComponent<Animator>();

        // Initialize train position based on scene
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        if (sceneIndex == 1 || sceneIndex == 2 || sceneIndex == 3)
        {
            train.transform.position = new Vector3(-13, train.transform.position.y, train.transform.position.z);
            StartCoroutine(AnimateTrainEntry());
        }
        else
        {
            playerCamera.transform.position = cameraGameplayPosition; // Ensure the camera is in the gameplay position for other scenes
        }
    }

    private void Update()
    {
        if (train.transform.position.x == 8f)
        {
            timer.stopTimer = false;
            // AkSoundEngine.PostEvent("Stop_chugga", gameManager.gameObject);

        }

        else
        {
            timer.stopTimer = true;
            switchCamera.SetAcceptInput(false);
        }

    }

    private IEnumerator AnimateTrainEntry()
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        if (sceneIndex == 1)
        {
            AkSoundEngine.PostEvent("Play_forest_bg", gameManager.gameObject);
            Debug.Log("Play Forest Event posted");
        }
        else if (sceneIndex == 2)
        {
            AkSoundEngine.PostEvent("Play_desert_bg", gameManager.gameObject);
        }
        else if (sceneIndex == 3)
        {
            AkSoundEngine.PostEvent("Play_coast_bg", gameManager.gameObject);
        }
        
        playerCamera.transform.position = cameraAnimationPosition;
        float duration = 5f;
        Vector3 targetTrainPosition = new Vector3(8, train.transform.position.y, train.transform.position.z);

        yield return MoveTrain(targetTrainPosition, duration);

        
        trainAnimator.SetBool("isStanding", true);

        player.SetActive(true); // Make the player appear
        playerMovement.enabled = true; // Allow player movement
        player.transform.position = new Vector3(0.3f, 1.7f, -9f);
        Debug.Log("Player to Start Position");
        StartCoroutine(AnimateCameraTransition(playerCamera.transform.position, cameraGameplayPosition,
            1f));
    }
   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") && playerMovement.letterDelivered)
        {
            playerMovement.enabled = false;
            playerMovement.HideMark();
            switchCamera.ChangeToCamera2();
            StartCoroutine(AnimateTrainExit());
        }
    }

    private IEnumerator AnimateTrainExit()
    {
        trainAnimator.SetBool("isStanding", false);
        StartCoroutine(AnimateCameraTransition(playerCamera.transform.position, cameraAnimationPosition, 1f));
        playerCamera.transform.position = cameraAnimationPosition;
        float duration = 5f;
        Vector3 targetTrainPosition = new Vector3(40, train.transform.position.y, train.transform.position.z);
        player.SetActive(false); // Make the player disappear
        yield return MoveTrain(targetTrainPosition, duration);
        gameManager.ShowWinOverlay();
        
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;
        AkSoundEngine.PostEvent("Play_chugga", gameManager.gameObject);

        if (sceneIndex == 1)
        {
            AkSoundEngine.PostEvent("Stop_forest_bg", gameManager.gameObject);
            Debug.Log("Stop Forest Event posted");
        }
        else if (sceneIndex == 2)
        {
            AkSoundEngine.PostEvent("Stop_desert_bg", gameManager.gameObject);
        }
        else if (sceneIndex == 3)
        {
            AkSoundEngine.PostEvent("Stop_coast_bg", gameManager.gameObject);
        }
    }

    private IEnumerator MoveTrain(Vector3 targetPosition, float duration)
    {
        

        Vector3 startPosition = train.transform.position;
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            float smoothT = t * t * (3f - 2f * t); // Smoothstep for easing
            train.transform.position = Vector3.Lerp(startPosition, targetPosition, smoothT);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        train.transform.position = targetPosition;
    }

    private IEnumerator AnimateCameraTransition(Vector3 startPosition, Vector3 targetPosition, float duration)
    {
        float elapsedTime = 0;

        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            playerCamera.transform.position = Vector3.Lerp(startPosition, targetPosition, t);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        playerCamera.transform.position = targetPosition;
        switchCamera.SetAcceptInput(true);
    }
}