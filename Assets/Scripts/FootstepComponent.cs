using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepComponent : MonoBehaviour
{
    private PlayerMovement _playerMovement;
    
    // Start is called before the first frame update
    void Start()
    {
        _playerMovement = GetComponentInParent<PlayerMovement>();
        if (_playerMovement == null)
        {
            Debug.LogError("PlayerMovement component not found on parent object.");
        }
    }
    
    void FootStepEvent()
    {
        if (_playerMovement != null && _playerMovement.currentSurfaceType != null)
        {
            // Set the switch based on the current surface type
            AkSoundEngine.SetSwitch("steps", _playerMovement.currentSurfaceType, gameObject);

            // Post the footstep event
            AkSoundEngine.PostEvent("Play_steps_switch", gameObject);
        }
        else
        {
            Debug.LogWarning("No valid surface detected, footstep event not played.");
        }
    }
    
}
