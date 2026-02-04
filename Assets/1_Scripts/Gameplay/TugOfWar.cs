using System.Collections;
using UnityEngine;


public class TugOfWar : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform _playerLeft;
    [SerializeField] private Transform _playerRight;

    [Header("Forces")]
    [SerializeField] private float _pullForce = 0.02f;
    [SerializeField] private float _verticalBonus = 1.5f;

    [Header("Movement")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float minSwipeDistancePercent = 0.05f;


    private Vector2? startPos = null;

    #region  abonement
    private void OnEnable()
    {
        MultiTouchSwipeDetector.OnSwipe += OnSwipe;
    }

    private void OnDisable()
    {
        MultiTouchSwipeDetector.OnSwipe -= OnSwipe;
    }
    #endregion

    private void OnSwipe(SwipeData swipe)
    {
        if (swipe.direction == SwipeDirection.Left)
        {
            MoveLeft();
        }
        else if (swipe.direction == SwipeDirection.Right)
        {
            MoveRight();
        }

        Vector3 startWorld = ScreenToWorld(swipe.startPos);
        Vector3 endWorld = ScreenToWorld(swipe.endPos);

        DrawSwipeLine(startWorld, endWorld);
    }


    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 screenPositionWithZ = new Vector3(screenPos.x, screenPos.y, 10f);
        return Camera.main.ScreenToWorldPoint(screenPositionWithZ);
    }
    // depalcements
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




    [SerializeField] private LineRenderer swipeLinePrefab;

    public void DrawSwipeLine(Vector3 startWorld, Vector3 endWorld)
    {
        if (swipeLinePrefab == null) return;

        LineRenderer line = Instantiate(swipeLinePrefab);
        line.positionCount = 2;
        line.SetPosition(0, startWorld);
        line.SetPosition(1, endWorld);

        StartCoroutine(DestroyLineAfterSeconds(line, 1f));
    }

    private IEnumerator DestroyLineAfterSeconds(LineRenderer line, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (line != null) Destroy(line.gameObject);
    }
}
