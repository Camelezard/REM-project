using UnityEngine;

public class SwapPlayerPositionEffect : TileEffect
{



    private void OnValidate()
    {
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
