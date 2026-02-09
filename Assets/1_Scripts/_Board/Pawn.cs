using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem.iOS;
using UnityEngine.Splines;

public class Pawn : MonoBehaviour
{
    // codex

    [SerializeField] private SplineContainer _SplineContainer;
    [SerializeField] private float _MoveDuration = 1f;
    [SerializeField] private float _TimePerMove = 1f;
    [SerializeField] private float _TransitionTimeOnTile = .1f;

    private int _CurrentTile = 10;

    void Start()
    {
        transform.position = TilePlacer.Instance.spawnedTiles[_CurrentTile].transform.position;

        StartCoroutine(MoveToTile(0));
    }

    // la coroutine va nous permetre de depalcer le pion jusque a la case voulu
    private IEnumerator MoveToTile(int pTargetTileIndex)
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
    }

    // Trasitione entre deux tiles ou qu'elle soit. 
    private IEnumerator MoveBetweenTwoTiles(int pOriinTileIndex, int pFinalTileIndex)
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
}