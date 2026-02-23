using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Vector3 _Offset = new Vector3(0, 10, -8);
    [SerializeField] private float _SmoothSpeed = 5f;

    private Vector3 _DesiredPosition; 

    public Transform target;
    public bool followTarget = false;

    private Coroutine _CurrentFollowPawnCoroutine;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }


    }

    private void LateUpdate()
    {
        if (!followTarget || target == null) return;

        _DesiredPosition = target.position + _Offset;

        transform.position = Vector3.Lerp(transform.position, _DesiredPosition, _SmoothSpeed * Time.deltaTime);
    }

    public void FollowPlayer(Pawn pPawn)
    {
        if (_CurrentFollowPawnCoroutine  != null) StopCoroutine(_CurrentFollowPawnCoroutine);
        _CurrentFollowPawnCoroutine = StartCoroutine(FollowPawnCoroutine(pPawn));
    }

    private IEnumerator FollowPawnCoroutine(Pawn pPawn)
    {
        yield return null;
    }
}
