using UnityEngine;

public class MinigameTileEffect : TileEffect
{
    [SerializeField] private TypOfMinigame _SelectedMinigame = TypOfMinigame.Random;

    public override void Execute(Pawn pPawn)
    {
        GameManager.GetInstance().StartMinigame(_SelectedMinigame);
    }

    public override string GetDescription()
    {
        return $"Request to lunch a minigame : {_SelectedMinigame}";
    }
    
    
}
