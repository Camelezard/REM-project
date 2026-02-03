using System;
using System.Collections.Generic;
using UnityEngine;
using Touch = UnityEngine.InputSystem.EnhancedTouch.Touch;
using TouchPhase = UnityEngine.InputSystem.TouchPhase;


public enum SwipeDirection
{
    Up,
    Down,
    Left,
    Right
}

public class MultiTouchSwipeDetector : MonoBehaviour
{

    public static Action<int, SwipeDirection, Vector2> OnSwipe;

    [SerializeField, Range(0.02f, 0.3f)]
    private float minSwipeScreenPercent = 0.1f;

    private Dictionary<int, Vector2> startPositions = new();

    void Update()
    {
        foreach (var touch in Touch.activeTouches)
        {
            int id = touch.finger.index;

            if (touch.phase == TouchPhase.Began)
            {
                startPositions[id] = touch.screenPosition;
            }

            if (touch.phase == TouchPhase.Ended &&
                startPositions.TryGetValue(id, out Vector2 startPos))
            {
                Vector2 delta = touch.screenPosition - startPos;
                float minDistance = Screen.width * minSwipeScreenPercent;

                if (delta.magnitude >= minDistance)
                {
                    SwipeDirection direction = GetDirection(delta);
                    OnSwipe?.Invoke(id, direction, delta.normalized);
                }

                startPositions.Remove(id);
            }
        }
    }

    SwipeDirection GetDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        else
            return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
    }
}