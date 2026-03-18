using UnityEngine;
using UnityEngine.U2D.Animation;
using System.Collections.Generic;

public class ModelCreator : MonoBehaviour
{
    [Header("Resolvers")]
    public SpriteResolver headResolver;
    public SpriteResolver bodyResolver;
    public SpriteResolver LeftArmResolver;
    public SpriteResolver RightArmResolver;
    public SpriteResolver RightLegResolver;
    public SpriteResolver LeftLegsolver;




    public void ApplyRandomOutfit(string pRandomLabel)
    {
        print($"label = {pRandomLabel}");

        headResolver.SetCategoryAndLabel("Hat", pRandomLabel);
        bodyResolver.SetCategoryAndLabel("Torso_Clothe", pRandomLabel);
        RightArmResolver.SetCategoryAndLabel("RightArm_Clothe", pRandomLabel);
        LeftArmResolver.SetCategoryAndLabel("LeftArm_Clothe", pRandomLabel);
        RightLegResolver.SetCategoryAndLabel("RightLeg_Clothe", pRandomLabel);
        LeftLegsolver.SetCategoryAndLabel("LeftLeg_Clothe", pRandomLabel);
    }
}
