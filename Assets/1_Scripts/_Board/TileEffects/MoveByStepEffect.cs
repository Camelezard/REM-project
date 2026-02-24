using UnityEngine;

public class MoveByStepEffect : TileEffect
{
    [SerializeField] private int _BonusStep = 3;



    private void OnValidate()
    {
        if (_BonusStep >= 0) m_EffectMessage = $"Effet activ� ! Avance le pion de {_BonusStep} cases.";
        else m_EffectMessage = $"Effet activ� ! Recule le pion de {_BonusStep} cases.";
        
        #if UNITY_EDITOR
        m_TileMaterial = UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/MoveByStepEffectColor.mat");
        ChangeColor(m_TileMaterial);
        #endif
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
