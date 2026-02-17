using UnityEngine;

public class MoveToSpecificTIleEffect : TileEffect
{
    [SerializeField] private int _TileIndex = 10;

    public override void Execute(Pawn pPawn)
    {
        StartCoroutine(pPawn.MoveBetweenTwoTiles(pPawn._CurrentTile, _TileIndex));
    }

    public override string GetDescription()
    {
        return $"Move the pawn to a specific tile. Tile destination : {_TileIndex}";
    }
    
}
