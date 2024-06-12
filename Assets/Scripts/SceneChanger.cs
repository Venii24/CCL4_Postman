using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToEnter;
    private PlayerMovement playerMovement;
    private LevelLoader levelLoader;
    private GameManager gameManager; // Reference to the GameManager

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        playerMovement = FindObjectOfType<PlayerMovement>(); // Find the PlayerMovement component in the scene
        gameManager = GameManager.Instance; // Get reference to the GameManager instance
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            // Open the UI Overlay
            playerMovement.enabled = false;
            gameManager.ShowWinOverlay(); // Show the WinOverlay UI element
            Invoke("ChangeScene", 2);
        }
    }
    
    private void ChangeScene()
    {
        SceneManager.LoadScene(sceneToEnter);
    }
}