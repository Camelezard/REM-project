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


    private IEnumerator MoveToTile(int pTargetTileIndex)
    {
        int lDirection = pTargetTileIndex > _CurrentTile ? 1 : -1;

        while (_CurrentTile != pTargetTileIndex)
        {
            int lNextTile = _CurrentTile + lDirection;

            yield return StartCoroutine(MoveBetweenTwoTiles(_CurrentTile, lNextTile));
            yield return new WaitForSeconds(_TimePerMove);

            _CurrentTile = lNextTile;
        }
    }

    private IEnumerator MoveBetweenTwoTiles(int pOriinTileIndex, int pFinalTileIndex)
    {
        float lElapsedTime = 0f;
        float lDistanceOnSpline;
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