using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToEnter;
    [SerializeField] public GameObject train;
    [SerializeField] public GameObject player;
    [SerializeField] public GameObject playerCamera;

    private Timer timer;
    private PlayerMovement playerMovement;
    private SwitchCamera switchCamera;
    private GameManager gameManager;
    private CollectableManager collectableManager;
   // private Texture StampImage;


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
        }

        else
        {
            timer.stopTimer = true;
            switchCamera.SetAcceptInput(false);
        }

    }

    private IEnumerator AnimateTrainEntry()
    {
        playerCamera.transform.position = cameraAnimationPosition;
        float duration = 5f;
        Vector3 targetTrainPosition = new Vector3(8, train.transform.position.y, train.transform.position.z);

        yield return MoveTrain(targetTrainPosition, duration);

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
            //StampImage = collectableManager.ReturnStampImage(gameManager.GetScore());
            switchCamera.ChangeToCamera2();
            StartCoroutine(AnimateTrainExit());
        }
    }

    private IEnumerator AnimateTrainExit()
    {
        StartCoroutine(AnimateCameraTransition(playerCamera.transform.position, cameraAnimationPosition, 1f));
        playerCamera.transform.position = cameraAnimationPosition;
        float duration = 5f;
        Vector3 targetTrainPosition = new Vector3(40, train.transform.position.y, train.transform.position.z);
        player.SetActive(false); // Make the player disappear
        yield return MoveTrain(targetTrainPosition, duration);
        
        gameManager.ShowWinOverlay();
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