using UnityEngine;

public class MinigameEffect : TileEffect
{
    [SerializeField] private TypOfMinigame _SelectedMinigame = TypOfMinigame.Random;

    private void OnValidate()
    {
        m_EffectMessage = $"Oh ? C'est l'heure de faire un minijeu !";
    }

    protected override void ExecuteEffect(Pawn pPawn)
    {
        GameManager.GetInstance().StartMinigame(_SelectedMinigame);
    }

    public override string GetDescription()
    {
        return $"Request to launch a minigame : {_SelectedMinigame}";
    }
    
    
}
