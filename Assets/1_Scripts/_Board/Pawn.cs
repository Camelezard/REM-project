using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Pawn : MonoBehaviour
{
    [SerializeField] private Spinner _Spinner;
    [SerializeField] private float _MoveDuration = 1f;
    [SerializeField] private float _TimePerMove = 0.5f;
    [SerializeField] private float _TransitionTimeOnTile = .1f;
    [SerializeField] private float _Spacing = 1.1f;
    [SerializeField] private float _OfsetSpeed = 0.2f;
    [SerializeField] private LayerMask _TileLayermask;

    private Vector3 offset;
    private Vector3 basePos;

    private BoardTile originTile;
    private BoardTile targetTile;

    public SplineContainer _SplineContainer;

    private Coroutine _moveOffsetRoutine;
    private Coroutine _rotateRoutine;

    public bool canEndTurn = false;
    public bool isOnEffectTile;

    private GameObject _BoardGround;

    public int currentTile { get; private set; } = 0;

    void Start()
    {
        if (_Spinner == null) _Spinner = Spinner.instance;

        BoardTile lTile = TilePlacer.Instance.spawnedTiles[currentTile];
        lTile.AddPawn(this);

        basePos = lTile.transform.position;
        offset = GetOffsetOnTile(lTile);

        transform.position = basePos + offset;

        _BoardGround = GameObject.FindGameObjectWithTag("Board");
    }

    private void OnDisable()
    {
        Spinner.OnSpinnerStopAtNumber -= MoveAfterSpinner;
    }

    public void StartTurn()
    {
        Spinner.OnSpinnerStopAtNumber += MoveAfterSpinner;
        _Spinner.SpinWheel();
    }

    public void EndTurn()
    {
        Spinner.OnSpinnerStopAtNumber -= MoveAfterSpinner;
        BoardManager.OnPlayerFinishTurn?.Invoke();
        canEndTurn = false;

        if (_BoardGround == null)
        {
            _BoardGround = GameObject.FindGameObjectWithTag("Board");
        }

        if (_BoardGround != null)
        {
            CameraManager.Instance.UpdateTarget(BoardManager.Instance.GetBoardCenter());
        }
        else
        {
            Debug.LogWarning("BoardGround not found");
        }

        GameManager.GetInstance().NextPlayerTurn();
    }

    public IEnumerator MoveToTile(int pTargetTileIndex)
    {
        int lDirection = pTargetTileIndex > currentTile ? 1 : -1;
        Vector3 lNextPos;

        while (currentTile != pTargetTileIndex)
        {
            int lNextTile = currentTile + lDirection;

            lNextPos = TilePlacer.Instance.spawnedTiles[lNextTile].transform.position;

            RotateLeftRight(lNextPos);

            yield return StartCoroutine(MoveBetweenTwoTiles(currentTile, lNextTile));

            // le personnage stop un instant sur les cases pour donner un effet de jeu de plateau
            yield return new WaitForSeconds(_TimePerMove);

            currentTile = lNextTile;
        }

        if (!CheckTile())
        {
            EndTurn();
        }
    }

    public IEnumerator MoveBetweenTwoTiles(int pOriginTileIndex, int pFinalTileIndex, SplineContainer pSplineContainer = null)
    {
        float lElapsedTime = 0f;
        float lDistanceOnSpline;

        float lStartDistanceOnSpline;
        float lEndDistanceOnSpline;

        if (pSplineContainer != null)
        {
            lStartDistanceOnSpline = 0;
            lEndDistanceOnSpline = 1;
        }
        else
        {
            lStartDistanceOnSpline = TilePlacer.Instance.spawnedTiles[pOriginTileIndex].distanceOnPath;
            lEndDistanceOnSpline = TilePlacer.Instance.spawnedTiles[pFinalTileIndex].distanceOnPath;

            pSplineContainer = _SplineContainer;
        }

        while (lElapsedTime < _MoveDuration)
        {
            lElapsedTime += Time.deltaTime;

            lDistanceOnSpline = Mathf.Lerp(lStartDistanceOnSpline, lEndDistanceOnSpline, lElapsedTime / _MoveDuration);
            transform.position = pSplineContainer.EvaluatePosition(lDistanceOnSpline);

            yield return null;
        }

        originTile = TilePlacer.Instance.spawnedTiles[pOriginTileIndex];
        targetTile = TilePlacer.Instance.spawnedTiles[pFinalTileIndex];

        originTile.RemovePawn(this);
        UpdateAllPawnsOffset(originTile);

        targetTile.AddPawn(this);
        UpdateAllPawnsOffset(targetTile);

        basePos = pSplineContainer.EvaluatePosition(lEndDistanceOnSpline);
        offset = GetOffsetOnTile(targetTile);

        transform.position = basePos + offset;
        currentTile = pFinalTileIndex;

        if (canEndTurn) EndTurn();
    }

    private void MoveAfterSpinner(int pValue)
    {
        if (gameObject == null)
        {
            Debug.LogError("spiner go not found");
            return;
        }

        StartCoroutine(MoveToTile(currentTile + pValue));
    }

    private bool CheckTile()
    {
        RaycastHit lHit;
        BoardTile lTile;
        bool lTileHasEffect = false;

        if (Physics.Raycast(transform.position + Vector3.up, Vector3.down, out lHit, 5f, _TileLayermask))
        {
            lTile = lHit.collider.gameObject.GetComponent<BoardTile>();
            lTileHasEffect = lTile.LaunchTileEffect(this);
            isOnEffectTile = lTileHasEffect;
        }

        Debug.DrawLine(transform.position, transform.position + Vector3.up * 100, Color.red, 3);

        return lTileHasEffect;
    }

    public void Cleanup()
    {
        Spinner.OnSpinnerStopAtNumber -= MoveAfterSpinner;
        StopAllCoroutines();
    }

    private void UpdateAllPawnsOffset(BoardTile pTile)
    {
        Vector3 lBasePos;
        Vector3 lOffset;

        foreach (var lPawn in pTile.pawnsOnTile)
        {
            lBasePos = pTile.transform.position;
            lOffset = lPawn.GetOffsetOnTile(pTile);

            lPawn.MoveToOffsetPosition(lBasePos + lOffset);
        }
    }

    public void MoveToOffsetPosition(Vector3 pTargetPos)
    {
        if (_moveOffsetRoutine != null)
            StopCoroutine(_moveOffsetRoutine);

        _moveOffsetRoutine = StartCoroutine(SmoothMove(pTargetPos));
    }

    private IEnumerator SmoothMove(Vector3 pTargetPos)
    {
        Vector3 lStartPos = transform.position;
        float lDuration = 0.2f;
        float lTime = 0f;

        while (lTime < lDuration)
        {
            lTime += Time.deltaTime;
            transform.position = Vector3.Lerp(lStartPos, pTargetPos, lTime / lDuration);
            yield return null;
        }

        transform.position = pTargetPos;
    }

    private Vector3 GetOffsetOnTile(BoardTile pTile)
    {
        int lCount = pTile.pawnsOnTile.Count;

        if (lCount <= 1)
            return Vector3.zero;

        int lIndex = pTile.pawnsOnTile.IndexOf(this);

        float lAngle = lIndex * Mathf.PI * 2 / lCount;

        Vector3 lOffset = new Vector3(Mathf.Cos(lAngle), 0, Mathf.Sin(lAngle)) * _Spacing;

        return lOffset;
    }



    public void RotateLeftRight(Vector3 pTargetPosition)
    {
        if (_rotateRoutine != null)
            StopCoroutine(_rotateRoutine);

        _rotateRoutine = StartCoroutine(SmoothRotateLeftRight(pTargetPosition));
    }

    private IEnumerator SmoothRotateLeftRight(Vector3 pTargetPosition)
    {
        Quaternion lStartRot = transform.rotation;

        Vector3 lDirection = (pTargetPosition - transform.position);

        float lTargetY = lDirection.x > 0 ? 0f : 180f;

        Quaternion lTargetRot = Quaternion.Euler(0, lTargetY, 0);

        float lDuration = 0.5f;
        float lTime = 0f;
        float lT;

        while (lTime < lDuration)
        {
            lTime += Time.deltaTime;

            lT = lTime / lDuration;
            lT = lT * lT * (3f - 2f * lT);

            transform.rotation = Quaternion.Slerp(lStartRot, lTargetRot, lT);

            yield return null;
        }

        transform.rotation = lTargetRot;
    }
}