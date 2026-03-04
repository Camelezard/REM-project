using UnityEngine;


public class MinigameEffect : TileEffect
{
    [SerializeField] private TypOfMinigame _SelectedMinigame = TypOfMinigame.Random;
    [SerializeField] int _BonusStep  = -3;

    private Pawn _Pawn;


    private void OnValidate()
    {
        m_EffectMessage = $"Oh ? C'est l'heure de jouer à un minijeu !";

#if UNITY_EDITOR
        m_TileMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/MinigameEffectColeor.mat");
        ChangeColor(m_TileMaterial);
#endif
    }


    protected override void ExecuteEffect(Pawn pPawn)
    {
        _Pawn = pPawn;
        BoardManager.OnMinigameFinished += OnMiniGameEnd;

        GameManager.GetInstance().StartMinigame(_SelectedMinigame);
    }

    void OnMiniGameEnd()
    {
        _Pawn.MoveToTile(_Pawn.currentTile + _BonusStep);
        BoardManager.OnMinigameFinished -= OnMiniGameEnd;
    }



    public override string GetDescription()
    {
        return $"Request to launch a minigame : {_SelectedMinigame}";
    }

}
