using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    [SerializeField] private string sceneToEnter;
    private PlayerMovement playerMovement;
    private LevelLoader levelLoader;

    void Start()
    {
        GetComponent<MeshRenderer>().enabled = false;
        playerMovement = FindObjectOfType<PlayerMovement>(); // Find the PlayerMovement component in the scene
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player entered the scene changer");
            Debug.Log("Letter delivered: " + playerMovement.letterDelivered);
            // Check if the player has delivered the letter
            if (playerMovement.letterDelivered)
            {
                // Load the next scene
                SceneManager.LoadScene(sceneToEnter);
            }
        }
    }
}