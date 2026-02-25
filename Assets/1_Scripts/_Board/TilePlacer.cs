using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.Tilemaps;
using Unity.VisualScripting;



#if UNITY_EDITOR
using UnityEditor;
#endif

public class TilePlacer : MonoBehaviour
{
    [SerializeField] private SplineContainer _TileSplineContainer;
    [SerializeField] private BoardTile _TileObject;
    [SerializeField] private int _TileCount = 10;

    [Header("Editor")]
    [SerializeField] private bool _CanGenerateTiles = false;
    [SerializeField] private bool _AdjustTiles = false;

    [SerializeField] private List<BoardTile> _SpawnedTiles;
    public List<BoardTile> spawnedTiles => _SpawnedTiles;
    Spline _TileSpline;

    #region Debug istance

    // doit etre separer en deux script
    public static TilePlacer Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    #endregion

#if UNITY_EDITOR
    void OnValidate()
    {
        if (_CanGenerateTiles)
        {
            _CanGenerateTiles = false;

            EditorApplication.delayCall += () =>
            {
                if (this != null)
                    GenerateTiles();
            };
        }


        if (_AdjustTiles)
        {
            _AdjustTiles = false;

            AdjustTiles();
        }
    }
#endif

    private void GenerateTiles()
    {
        BoardTile lNewTile;

        _TileSpline = _TileSplineContainer.Spline;

        if (_SpawnedTiles == null) _SpawnedTiles = new List<BoardTile>();

        ClearTiles();

        for (int i = 0; i < _TileCount; i++)
        {
            lNewTile = Instantiate(_TileObject, transform);

            lNewTile.distanceOnPath = i / (float)(_TileCount - 1);

            _SpawnedTiles.Add(lNewTile);

            lNewTile._TileSplineContainer = _TileSplineContainer;

            lNewTile.AdjustOnSline();

            lNewTile.ChangeText(i);

            lNewTile.name = $"BoardTile_{i}";
        }


        _CanGenerateTiles = false;
        
    }

    private void AdjustTiles()
    {
        if (_SpawnedTiles == null || _SpawnedTiles.Count < 1) return;

        BoardTile lTile;
        _TileCount = _SpawnedTiles.Count;

        for (int i = 0; i < _TileCount; i++)
        {
            lTile = _SpawnedTiles[i];

            lTile.AdjustOnSline();
        }
    }

    private void ClearTiles()
    {
#if UNITY_EDITOR
        foreach (var tile in _SpawnedTiles)
        {
            if (tile != null)
                DestroyImmediate(tile.gameObject);
        }
#else
        foreach (var tile in _spawnedTiles)
        {
            if (tile != null)
                Destroy(tile.gameObject);
        }
#endif

        _SpawnedTiles.Clear();
    }
}