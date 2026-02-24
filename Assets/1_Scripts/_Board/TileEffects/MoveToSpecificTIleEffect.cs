using UnityEngine;

public class MoveToSpecificTileEffect : TileEffect
{
    [SerializeField] private int _TileIndex = 10;

    private void OnValidate()
    {
        m_EffectMessage = $"Effet activé ! Déplace le pion jusqu'à la case {_TileIndex}.";
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
