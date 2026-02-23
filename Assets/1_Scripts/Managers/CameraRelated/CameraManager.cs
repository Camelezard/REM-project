using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private float _ZoomInDuration;
    [SerializeField] private float _ZoomOutDuration;

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
