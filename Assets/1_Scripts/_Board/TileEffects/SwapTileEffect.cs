using UnityEngine;

public class SwapTileEffect : TileEffect
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnValidate()
    {
        m_EffectMessage = $"Effet activé ! Les pions échangent leur cases.";
    }

    protected override void ExecuteEffect(Pawn pPawn)
    {
        Pawn lPawn1 = BoardManager.Instance.pawnList[0];
        Pawn lPawn2 = BoardManager.Instance.pawnList[1];

        StartCoroutine(lPawn1.MoveBetweenTwoTiles(lPawn1.currentTile, lPawn2.currentTile));
        StartCoroutine(lPawn2.MoveBetweenTwoTiles(lPawn2.currentTile, lPawn1.currentTile));
    }
}
