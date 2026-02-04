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
    public Vector2 startPos;
    public Vector2 endPos;
    public Vector2 currentPos;
    public Vector2 delta;
    public Vector2 velocity;
    public float distance;
    public float duration;
    public SwipeDirection direction;
}

public class MultiTouchSwipeDetector : MonoBehaviour
{
    public static Action<SwipeData> OnSwipeStart;
    public static Action<SwipeData> OnSwipeUpdate;
    public static Action<SwipeData> OnSwipeEnd;


    [SerializeField, Range(0.02f, 0.3f)]
    private float minSwipeScreenPercent = 0.1f;
    private int _FingerId;

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

    private Dictionary<int, Vector2> lastPositions = new();
    private Dictionary<int, float> lastTimes = new();

    private void GetTouche()
    {
        foreach (var touch in Touch.activeTouches)
        {
            _FingerId = touch.finger.index;

            // on touch Began
            if (touch.phase == TouchPhase.Began)
            {
                startPositions[_FingerId] = touch.screenPosition;
                startTimes[_FingerId] = Time.time;
                lastPositions[_FingerId] = touch.screenPosition;
                lastTimes[_FingerId] = Time.time;

                OnSwipeStart?.Invoke(new SwipeData
                {
                    startPos = touch.screenPosition,
                    currentPos = touch.screenPosition
                });
            }


            // on touch Move
            if (touch.phase == TouchPhase.Moved)
            {
                Vector2 lastPos = lastPositions[_FingerId];
                float lastTime = lastTimes[_FingerId];

                Vector2 delta = touch.screenPosition - lastPos;
                float dt = Time.time - lastTime;

                SwipeData data = new SwipeData
                {
                    startPos = startPositions[_FingerId],
                    currentPos = touch.screenPosition,
                    delta = delta,
                    velocity = delta / Mathf.Max(dt, 0.0001f),
                    distance = Vector2.Distance(startPositions[_FingerId], touch.screenPosition),
                    duration = Time.time - startTimes[_FingerId],
                    direction = GetDirection(touch.screenPosition - startPositions[_FingerId])
                };

                OnSwipeUpdate?.Invoke(data);

                lastPositions[_FingerId] = touch.screenPosition;
                lastTimes[_FingerId] = Time.time;
            }


            // on touch Move
            if (touch.phase == TouchPhase.Ended)
            {
                SwipeData data = new SwipeData
                {
                    startPos = startPositions[_FingerId],
                    currentPos = touch.screenPosition,
                    distance = Vector2.Distance(startPositions[_FingerId], touch.screenPosition),
                    duration = Time.time - startTimes[_FingerId],
                    direction = GetDirection(touch.screenPosition - startPositions[_FingerId])
                };

                OnSwipeEnd?.Invoke(data);

                startPositions.Remove(_FingerId);
                startTimes.Remove(_FingerId);
                lastPositions.Remove(_FingerId);
                lastTimes.Remove(_FingerId);
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
