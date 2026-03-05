using System.Collections;
using UnityEngine;
using UnityEngine.Splines;

public class Pawn : MonoBehaviour
{
    // codex

    [SerializeField] private Spinner _Spinner;
    [SerializeField] private float _MoveDuration = 1f;
    [SerializeField] private float _TimePerMove = 0.5f;
    [SerializeField] private float _TransitionTimeOnTile = .1f;
    [SerializeField] private LayerMask _TileLayermask;

    public SplineContainer _SplineContainer;
    public bool canEndTurn = false;
    public bool isOnEffectTile;

    private GameObject _BoardGround;

    public int currentTile { get; private set; } = 0;

    void Start()
    {
        if (_Spinner == null) _Spinner = Spinner.instance;


        transform.position = TilePlacer.Instance.spawnedTiles[currentTile].transform.position;

        _BoardGround = GameObject.FindGameObjectWithTag("Board");
        //_Spinner.SpinWheel();
        //StartCoroutine(MoveToTile(_CurrentTile + 3));
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

        CameraManager.Instance.UpdateTarget(_BoardGround.transform);
        GameManager.GetInstance().NextPlayerTurn();
    }


    // la coroutine va nous permetre de depalcer le pion jusque a la case voulu
    public IEnumerator MoveToTile(int pTargetTileIndex)
    {
        int lDirection = pTargetTileIndex > currentTile ? 1 : -1; // pour savoir si on avance ou si on recule

        while (currentTile != pTargetTileIndex)
        {
            int lNextTile = currentTile + lDirection;

            // transition entre deux tile
            yield return StartCoroutine(MoveBetweenTwoTiles(currentTile, lNextTile));

            // le personnage stop un instant sur les cases pour donner un effet de jeu de plateau
            yield return new WaitForSeconds(_TimePerMove);

            currentTile = lNextTile;
        }

        //CheckTile();    // comenter pour test le flow je sais pas ou indique la fin du tour
        if (!CheckTile()) EndTurn();

    }

    // Trasitione entre deux tiles ou qu'elle soit. 
    public IEnumerator MoveBetweenTwoTiles(int pOriinTileIndex, int pFinalTileIndex, SplineContainer pSplineContainer = null)
    {
        float lElapsedTime = 0f;
        float lDistanceOnSpline;

        // determie la place en pourcent sur le spline
        float lStartDistanceOnSpline;
        float lEndDistanceOnSpline;

        if (pSplineContainer != null)
        {
            lStartDistanceOnSpline = 0;
            lEndDistanceOnSpline = 1;

        }
        else
        {
            lStartDistanceOnSpline = TilePlacer.Instance.spawnedTiles[pOriinTileIndex].distanceOnPath;
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

        transform.position = pSplineContainer.EvaluatePosition(lEndDistanceOnSpline);
        currentTile = pFinalTileIndex;

        if (canEndTurn) EndTurn();
        //else if (isOnEffectTile) CheckTile();
    }




    private void MoveAfterSpinner(int pValue)
    {
        StartCoroutine(MoveToTile(currentTile + pValue));
    }

    private bool CheckTile()
    {
        RaycastHit lHit;
        BoardTile lTile;
        bool lTileHasEffect = false;

        if (Physics.Raycast(transform.position + new Vector3(0, 1, 0), Vector3.down, out lHit, 5f, _TileLayermask))
        {
            lTile = lHit.collider.gameObject.GetComponent<BoardTile>();
            lTileHasEffect = lTile.LaunchTileEffect(this);
            isOnEffectTile = lTileHasEffect;
        }

        return lTileHasEffect;
    }
}