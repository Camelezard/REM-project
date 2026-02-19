using UnityEngine;

public class MoveByStepEffect : TileEffect
{
    [SerializeField] private int _BonusStep = 3;

    private void OnValidate()
    {
        if (_BonusStep >= 0) m_EffectMessage = $"Effet activé ! Avance le pion de {_BonusStep} cases.";
        else m_EffectMessage = $"Effet activé ! Recule le pion de {_BonusStep} cases.";
    }

    protected override void ExecuteEffect(Pawn pPawn)
    {
        StartCoroutine(pPawn.MoveToTile(pPawn._CurrentTile + _BonusStep));
    }

    public override string GetDescription()
    {
        return $"Move the pawn by number of steps. Negative Number make it go backward. Number of steps : {_BonusStep}";
    }
    
}
