using UnityEngine;

public class MoveToSpecificTileEffect : TileEffect
{
    [SerializeField] private int _TileIndex = 10;

    private void OnValidate()
    {
        m_EffectMessage = $"Effet activ� ! D�place le pion jusqu'� la case {_TileIndex}.";

        #if UNITY_EDITOR
        m_TileMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/MoveToSpecificTileEffectColor.mat");
        ChangeColor(m_TileMaterial);
        #endif
    }

    protected override void ExecuteEffect(Pawn pPawn)
    {
        if (!pPawn.isOnEffectTile) pPawn.canEndTurn = true;
        StartCoroutine(pPawn.MoveBetweenTwoTiles(pPawn._CurrentTile, _TileIndex));
    }

    public override string GetDescription()
    {
        return $"Move the pawn to a specific tile. Tile destination : {_TileIndex}";
    }
    
}
