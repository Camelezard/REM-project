using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Splines;

public class Pawn : MonoBehaviour
{
    // codex

    [SerializeField] private Spinner _Spinner;
    [SerializeField] private float _MoveDuration = 1f;
    [SerializeField] private float _TimePerMove = 1f;
    [SerializeField] private float _TransitionTimeOnTile = .1f;
    [SerializeField] private LayerMask _TileLayermask;
    
    public SplineContainer _SplineContainer ;

    public int _CurrentTile { get; private set;} = 0;

    void Start()
    {
        if(_Spinner == null) _Spinner = Spinner.instance; 


        transform.position = TilePlacer.Instance.spawnedTiles[_CurrentTile].transform.position;

        //_Spinner.SpinWheel();
        //StartCoroutine(MoveToTile(_CurrentTile + 3));
    }


    public void StartTurn()
    {
        Spinner.OnSpinnerStopAtNumber += MoveAfterSpinner;
        _Spinner.SpinWheel();
        
    }

    public void EndTurn()
    {
        Spinner.OnSpinnerStopAtNumber -= MoveAfterSpinner;
        BoardManager.OnpLplayerFinshTun?.Invoke();

        GameManager.GetInstance().NextPlayerTurn();

        Debug.Log($"{gameObject.name}TurnEnd");
    }

    // la coroutine va nous permetre de depalcer le pion jusque a la case voulu
    public IEnumerator MoveToTile(int pTargetTileIndex)
    {
        int lDirection = pTargetTileIndex > _CurrentTile ? 1 : -1; // pour savoir si on avance ou si on recule

        while (_CurrentTile != pTargetTileIndex)
        {
            int lNextTile = _CurrentTile + lDirection;

            // transition entre deux tile
            yield return StartCoroutine(MoveBetweenTwoTiles(_CurrentTile, lNextTile));

            // le personnage stop un instant sur les cases pour donner un effet de jeu de plateau
            yield return new WaitForSeconds(_TimePerMove);

            _CurrentTile = lNextTile;
        }

        //CheckTile();    // comenter pour test le flow je sais pas ou indique la fin du tour
        EndTurn();
    }

    // Trasitione entre deux tiles ou qu'elle soit. 
    public IEnumerator MoveBetweenTwoTiles(int pOriinTileIndex, int pFinalTileIndex)
    {
        float lElapsedTime = 0f;
        float lDistanceOnSpline;

        // determie la place en pourcent sur le spline
        float lStartDistanceOnSpline = TilePlacer.Instance.spawnedTiles[pOriinTileIndex].distanceOnPath;
        float lEndDistanceOnSpline = TilePlacer.Instance.spawnedTiles[pFinalTileIndex].distanceOnPath;

        while (lElapsedTime < _MoveDuration)
        {
            lElapsedTime += Time.deltaTime;
            
            lDistanceOnSpline = Mathf.Lerp(lStartDistanceOnSpline, lEndDistanceOnSpline, lElapsedTime / _MoveDuration);

            transform.position = _SplineContainer.EvaluatePosition(lDistanceOnSpline);

            yield return null;
        }

        transform.position = _SplineContainer.EvaluatePosition(lEndDistanceOnSpline);
    }

    private void MoveAfterSpinner(int pValue)
    {
        StartCoroutine(MoveToTile(_CurrentTile + pValue));
    }

    private void CheckTile()
    {
        RaycastHit lHit;
        BoardTile lTile;
        if (Physics.Raycast(transform.position + new Vector3(0,1,0), Vector3.down, out lHit, 5f, _TileLayermask))
        {
            lTile = lHit.collider.gameObject.GetComponent<BoardTile>();
            lTile.ExecuteEffect(this);
        }
    }
}