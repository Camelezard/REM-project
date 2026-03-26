using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;

public class Dodger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] protected float m_speed = 5f;
    protected Vector2 m_velocity;

    [SerializeField] protected float m_horizontalzonePercent = 0.5f;
    [SerializeField] protected float m_verticalZonePErcent = 0.5f;
    [SerializeField] protected PlayerZone m_PlayerZone = PlayerZone.Bottom;

    protected virtual void Start()
    {
        EnhancedTouchSupport.Enable();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        foreach (var touch in Touch.activeTouches)
        {
            Vector2 lTouchPos = touch.screenPosition;

            if (m_PlayerZone == PlayerZone.Bottom)
            {
                if (lTouchPos.x < Screen.width * m_horizontalzonePercent && lTouchPos.y < Screen.height * m_verticalZonePErcent)
                {
                    // Bottom left
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                    {
                        m_velocity = Vector2.left;
                    }
                    else
                    {
                        m_velocity = Vector2.zero;
                    }
                }

                if (lTouchPos.x > Screen.width * m_horizontalzonePercent && lTouchPos.y < Screen.height * m_verticalZonePErcent)
                {
                    // Bottom right
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                    {
                        m_velocity = Vector2.right;
                    }
                    else
                    {
                        m_velocity = Vector2.zero;
                    }
                }
            }

            else if (m_PlayerZone == PlayerZone.Top)
            {
                if (lTouchPos.x > Screen.width * m_horizontalzonePercent && lTouchPos.y > Screen.height * m_verticalZonePErcent)
                {
                    // Top right
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                    {
                        m_velocity = Vector2.right;
                    }
                    else
                    {
                        m_velocity = Vector2.zero;
                    }
                }

                if (lTouchPos.x < Screen.width * m_horizontalzonePercent && lTouchPos.y > Screen.height * m_verticalZonePErcent)
                {
                    // Top left
                    if (touch.phase == UnityEngine.InputSystem.TouchPhase.Stationary || touch.phase == UnityEngine.InputSystem.TouchPhase.Moved)
                    {
                        m_velocity = Vector2.left;
                    }
                    else
                    {
                        m_velocity = Vector2.zero;
                    }
                }
            }
        }

        transform.position += new Vector3(m_velocity.x, m_velocity.y, 0) * m_speed * Time.deltaTime;
    }
}
