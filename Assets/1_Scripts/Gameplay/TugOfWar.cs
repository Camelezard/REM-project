using UnityEngine;using UnityEngine;
using UnityEngine.InputSystem.EnhancedTouch;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;
public class TugOfWar : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform _playerLeft;
    [SerializeField] private Transform _playerRight;

    [Header("Forces")]
    [SerializeField] private float _pullForce = 0.02f;
    [SerializeField] private float _verticalBonus = 1.5f;

/// <summary>
///  testing
/// </summary>
    [SerializeField] private float moveSpeed = 5f; // vitesse du déplacement
    [SerializeField] private float minSwipeDistancePercent = 0.05f; // % de l'écran pour considérer un swipe

    private Vector2? startPos = null;

    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            if (touch.phase == TouchPhase.Began)
            {
                startPos = touch.screenPosition;
            }
            else if (touch.phase == TouchPhase.Ended && startPos.HasValue)
            {
                Vector2 delta = touch.screenPosition - startPos.Value;
                float minDistance = Screen.width * minSwipeDistancePercent;

                if (Mathf.Abs(delta.x) >= minDistance)
                {
                    if (delta.x > 0)
                        MoveRight();
                    else
                        MoveLeft();
                }

                startPos = null;
            }
        }
    }

    private void MoveRight()
    {
        transform.position += Vector3.right * moveSpeed * Time.deltaTime;
        Debug.Log("Déplacement droite");
    }

    private void MoveLeft()
    {
        transform.position += Vector3.left * moveSpeed * Time.deltaTime;
        Debug.Log("Déplacement gauche");
    }
}