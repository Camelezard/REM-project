using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Flippers : MonoBehaviour
{
    [SerializeField] private Transform _LeftFlipper;
    [SerializeField] private Transform _RightFlipper;

    private Quaternion _LeftFlipperStartRot;
    private Quaternion _RightFlipperStartRot;

    [SerializeField] private float _LeftFlipperEndRot;
    [SerializeField] private float _RightFlipperEndRot;
    [SerializeField] private float _RotationSpeed = 500f;

    private Quaternion _TargetRotQuatLeft;
    private Quaternion _TargetRotQuatRight;

    [Header("Zone Setting")]
    [SerializeField] private float horizontalZonePercent = 0.4f;
    [SerializeField] private float verticalZonePercent = 0.4f;

    [SerializeField] private int _PlayerID = 1;
    [SerializeField] private PlayerZone _PlayerZone;

    public enum PlayerZone
    {
        Bottom,
        Top
    }

    private void Awake()
    {
        EnhancedTouchSupport.Enable();

        _LeftFlipperStartRot = _LeftFlipper.localRotation;
        _RightFlipperStartRot = _RightFlipper.localRotation; 
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DetectTouches();
        AnimateFlippers();
    }

    private void DetectTouches()
    {
        foreach (var touch in Touch.activeTouches)
        {
            Vector2 lTouchPos = touch.screenPosition;

            if (_PlayerZone == PlayerZone.Bottom)
            {
                if (lTouchPos.x < Screen.width * horizontalZonePercent && lTouchPos.y < Screen.height * verticalZonePercent)
                {
                    // Bottom left
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        ActionateLeftFlipper(true);
                    }
                    else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        ActionateLeftFlipper(false);
                    }
                }

                if (lTouchPos.x > Screen.width * horizontalZonePercent && lTouchPos.y < Screen.height * verticalZonePercent)
                {
                    // Bottom right
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        ActionateRightFlipper(true);
                    }
                    else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        ActionateRightFlipper(false);
                    }
                }
            }

            else if (_PlayerZone == PlayerZone.Top)
            {
                if (lTouchPos.x > Screen.width * horizontalZonePercent && lTouchPos.y > Screen.height * verticalZonePercent)
                {
                    // Top right
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        ActionateLeftFlipper(true);
                    }
                    else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        ActionateLeftFlipper(false);
                    }
                }

                if (lTouchPos.x < Screen.width * horizontalZonePercent && lTouchPos.y > Screen.height * verticalZonePercent)
                {
                    // Top left
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Began)
                    {
                        ActionateRightFlipper(true);
                    }
                    else if (touch.phase == UnityEngine.InputSystem.TouchPhase.Ended)
                    {
                        ActionateRightFlipper(false);
                    }
                }
            }
        }
    }

    private void ActionateLeftFlipper(bool pValue)
    {
        if (pValue)
        {
            _TargetRotQuatLeft = Quaternion.Euler(0, 0, _LeftFlipperEndRot);
        }
        else
        {
            _TargetRotQuatLeft = _LeftFlipperStartRot;
        }
    }

    private void ActionateRightFlipper(bool pValue)
    {
        if (pValue)
        {
            _TargetRotQuatRight = Quaternion.Euler(0, 0, _RightFlipperEndRot);
        }
        else
        {
            _TargetRotQuatRight = _RightFlipperStartRot;
        }
    }

    private void AnimateFlippers()
    {
        _LeftFlipper.localRotation = Quaternion.RotateTowards(
            _LeftFlipper.localRotation,
            _TargetRotQuatLeft,
            _RotationSpeed * Time.deltaTime);

        _RightFlipper.localRotation = Quaternion.RotateTowards(
            _RightFlipper.localRotation,
            _TargetRotQuatRight,
            _RotationSpeed * Time.deltaTime);
    }

    private void OnDestroy()
    {
        EnhancedTouchSupport.Disable();
    }
}
