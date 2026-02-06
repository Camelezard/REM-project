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

    [Header("Til Caracteistique")]
    [Range(0f, 1f)] public float distanceOnPath;
    [SerializeField] public Spline spline;



    void OnValidate()
    {
        AdjustOnSline();
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
}
