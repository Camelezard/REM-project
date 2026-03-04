using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class Flippers : MonoBehaviour
{
    [SerializeField] private Transform _LeftFlipper;
    [SerializeField] private Transform _RightFlipper;

    [Header("Zone Setting")]
    [SerializeField] private float leftZonePercent = 0.3f;
    [SerializeField] private float rightZonePercent = 0.3f;

    [SerializeField] private int _PlayerID = 1;

    private void Awake()
    {
        EnhancedTouchSupport.Enable();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectTouches();
    }

    private void DetectTouches()
    {
        foreach (var touch in UnityEngine.InputSystem.EnhancedTouch.Touch.activeTouches)
        {
            Vector2 lTouchPos = touch.screenPosition;

            
        }
    }
}
