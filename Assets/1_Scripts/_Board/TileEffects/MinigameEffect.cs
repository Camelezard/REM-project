using UnityEngine;


public class MinigameEffect : TileEffect
{
    [SerializeField] private TypOfMinigame _SelectedMinigame = TypOfMinigame.Random;


    private void OnValidate()
    {
        m_EffectMessage = $"Oh ? C'est l'heure de jouer � un minijeu !";

        #if UNITY_EDITOR
        m_TileMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/MinigameEffectColeor.mat");
        ChangeColor(m_TileMaterial);
        #endif
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
