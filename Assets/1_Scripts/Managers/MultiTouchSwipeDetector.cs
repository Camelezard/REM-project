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

public struct SwipeData
{
    public int fingerId;
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 delta;
    public Vector2 directionNormalized;
    public float distance;
    public float duration;
    public SwipeDirection direction;
}

public class MultiTouchSwipeDetector : MonoBehaviour
{
    public static Action<SwipeData> OnSwipe;

    [SerializeField, Range(0.02f, 0.3f)]
    private float minSwipeScreenPercent = 0.1f;

    private Dictionary<int, Vector2> startPositions = new();
    private Dictionary<int, float> startTimes = new();

    private float minDistance;

    void Start()
    {
        minDistance = Screen.width * minSwipeScreenPercent;
    }

    void Update()
    {
        GetTouche();
    }

    private void GetTouche()
    {
        foreach (var touch in Touch.activeTouches)
        {
            int id = touch.finger.index;

            if (touch.phase == TouchPhase.Began)
            {
                startPositions[id] = touch.screenPosition;
                startTimes[id] = Time.time;
            }

            if (touch.phase == TouchPhase.Ended &&
                startPositions.TryGetValue(id, out Vector2 startPos))
            {
                Vector2 endPos = touch.screenPosition;
                Vector2 delta = endPos - startPos;
                float distance = delta.magnitude;

                if (distance >= minDistance)
                {
                    SwipeData data = new SwipeData
                    {
                        fingerId = id,
                        startPos = startPos,
                        endPos = endPos,
                        delta = delta,
                        distance = distance,
                        duration = Time.time - startTimes[id],
                        directionNormalized = delta.normalized,
                        direction = GetDirection(delta)
                    };

                    OnSwipe?.Invoke(data);
                }

                startPositions.Remove(id);
                startTimes.Remove(id);
            }
        }
    }

    private SwipeDirection GetDirection(Vector2 delta)
    {
        if (Mathf.Abs(delta.x) > Mathf.Abs(delta.y))
            return delta.x > 0 ? SwipeDirection.Right : SwipeDirection.Left;
        else
            return delta.y > 0 ? SwipeDirection.Up : SwipeDirection.Down;
    }
}
