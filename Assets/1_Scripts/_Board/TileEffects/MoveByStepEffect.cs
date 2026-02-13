using UnityEngine;

public class MoveByStepEffect : TileEffect
{
    [SerializeField] private int _BonusStep = 3;

    public override void Execute(Pawn pPawn)
    {
        StartCoroutine(pPawn.MoveToTile(pPawn.currentTile + _BonusStep));
    }

    public override string GetDescription()
    {
        return $"Move the pawn by number of steps. Negative Number make it go backward. Number of steps : {_BonusStep}";
    }
    
}
