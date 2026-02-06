using UnityEngine;
using UnityEngine.Splines;

public class BoardTile : MonoBehaviour
{

    public SplineContainer _TileSplineContainer;
    public Spline spline;
    [Range(0f, 1f)] public float distanceOnPath;



    void OnValidate()
    {
        AdjustOnSline();
    }

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
