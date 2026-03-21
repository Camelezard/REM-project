using UnityEngine;
using UnityEngine.Splines;

public class MoveToSpecificTileEffect : TileEffect
{
    [SerializeField] private int _TileIndex = 10;
    [SerializeField] private SplineContainer _AlernativeSpline = null;

    private void OnValidate()
    {
        m_EffectMessage = $"Effet activé ! Déplace le pion jusqu'à la case {_TileIndex}.";
        billboardEffectMessage = $"Déplace à la case {_TileIndex}";

        #if UNITY_EDITOR
        m_TileMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/MoveToSpecificTileEffectColor.mat");
        ChangeColor(m_TileMaterial);
        #endif
    }

    protected override void ExecuteEffect(Pawn pPawn)
    {
        /*if (!pPawn.isOnEffectTile)*/ pPawn.canEndTurn = true;
        StartCoroutine(pPawn.MoveBetweenTwoTiles(pPawn.currentTile, _TileIndex,_AlernativeSpline));
    }

    public override string GetDescription()
    {
        return $"Move the pawn to a specific tile. Tile destination : {_TileIndex}";
    }
    
}
