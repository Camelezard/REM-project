using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;

public class TouchBootstrap : MonoBehaviour
{
    void OnEnable()
    {
        EnhancedTouchSupport.Enable();
    }

    void OnDisable()
    {
        EnhancedTouchSupport.Disable();
    }
}
