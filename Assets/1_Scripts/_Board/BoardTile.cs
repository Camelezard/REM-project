using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BoardTile : MonoBehaviour
{

    private enum TileType
    {
        StartTile,
        GameTile,
        RandoGameTile,
        EffectTile,
        EndTile
    }

    public SplineContainer _TileSplineContainer;
    private TileType tileType;

    [Header("Tile Specifications")]
    [Range(0f, 1f)] public float distanceOnPath;
    [SerializeField] public Spline spline;

    private List<TileEffect> _Effects = new List<TileEffect>();

    private void Awake()
    {
        _Effects.AddRange(GetComponents<TileEffect>());
    }

    void OnValidate()
    {
        AdjustOnSline();

        _Effects.Clear();
        _Effects.AddRange(GetComponents<TileEffect>());

        string lDescription;
        foreach (TileEffect lEffect in _Effects)
        {
            lDescription = "- " + lEffect.GetDescription() + "\n";
        }
    }

    /// <summary>
    /// 
    /// permet d'ajuster la position de la tile pour les raprocher ou les ecarter
    /// <summary>
    public void AdjustOnSline()
    {
        if (_TileSplineContainer == null)
        {
            Debug.Log("_TileSplineContainer not assigned");
            return;
        }

        transform.position = _TileSplineContainer.EvaluatePosition(distanceOnPath);
    }

    public void ExecuteEffect(Pawn pPawn)
    {
        if (_Effects.Count == 0) return;
        foreach (TileEffect lEffect in _Effects)
        {
            lEffect.Execute(pPawn);
        }
    }
}
