using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class LateralMovement : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float _HorizontalZonePercent = 0.4f;
    [SerializeField] private float _HorizontalSpeed;
    [SerializeField] private float _HorizontalAccForce;
    private float _HorizontalAcceleration;
    private float _HorizontalVelocity;
    void Start()
    {
        EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        DetectTouches();
        Movement();
    }

    private void DetectTouches()
    {
        foreach (var touch in Touch.activeTouches)
        {
            Vector2 lTouchPos = touch.screenPosition;

            if (lTouchPos.x < Screen.width * _HorizontalZonePercent)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    _HorizontalVelocity = -1;
                    _HorizontalAcceleration += _HorizontalAccForce * Time.deltaTime;
                }
                else
                {
                    _HorizontalVelocity = 0;
                    _HorizontalAcceleration = 0;
                }
            }

            else if (lTouchPos.x > Screen.width * _HorizontalZonePercent)
            {
                if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                {
                    _HorizontalVelocity = 1;
                    _HorizontalAcceleration += _HorizontalAccForce * Time.deltaTime;
                }
                else
                {
                    _HorizontalVelocity = 0;
                    _HorizontalAcceleration = 0;
                }
            }
        }
    }

    private void Movement()
    {
        _HorizontalAcceleration = Mathf.Clamp(_HorizontalAcceleration, 0, _HorizontalSpeed);
        transform.position += new Vector3(_HorizontalVelocity, 0, 0) * _HorizontalAcceleration * Time.deltaTime;
    }
}
