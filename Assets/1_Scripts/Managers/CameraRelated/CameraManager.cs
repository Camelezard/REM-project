using System.Collections;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private Vector3 _Offset = new Vector3(0, 10, -8);
    [SerializeField] private Vector3 _BoardOffset = new Vector3(0, 10, -8);
    [SerializeField] private float _SmoothSpeed = 5f;

    private Vector3 _DesiredPosition;

    private const string BOARD_TAG = "Board";

    public Transform target;
    public bool followTarget = false;

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

        if (target.tag == BOARD_TAG) _DesiredPosition = target.position + _BoardOffset;
        else _DesiredPosition = target.position + _Offset;

        transform.position = Vector3.Lerp(
            transform.position,
            _DesiredPosition,
            _SmoothSpeed * Time.deltaTime);
    }

    public void UpdateTarget(Transform pNewTarget, bool pNewFollowTarget = true)
    {
        target = pNewTarget;
        followTarget = pNewFollowTarget;
    }
}
