using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public enum DressLabelTypeEnum
{
    Mousquetaire,
    Clown,
    Knight,
    Cowboy,
    REM_Clothesv2,
    Hair_1,
    Hair_2,
    Hair_3,
    Hair_4,
    dark_skin,
    light_skin

};
public enum BoddyPartLabelTypeEnum
{
    Head,
    Torso,
    RightLeg,
    LeftLeg,
    RightArm,
    LeftArm,
    Hat,
    Torso_Clothe,
    RightLeg_Clothe,
    LeftLeg_Clothe,
    RightArm_Clothe,
    LeftArm_Clothe,
    FaceExpression
};
public enum SkinLabelTypeEnum
{
    light_skin,
    dark_skin
}

public class CharacterCustomizer : MonoBehaviour
{

    public static CharacterCustomizer instance;
    [SerializeField] public CharacterPoseController poseControler;
    [SerializeField] public float ReactionTime = 1.5f;
    [SerializeField] public SpriteRenderer legSprite;

    private List<DressLabelTypeEnum> coverLegClothing = new List<DressLabelTypeEnum> { DressLabelTypeEnum.Knight };

    [Header("Resolvers")]
    public SpriteResolver hatResolver;
    public SpriteResolver bodyDressResolver;
    public SpriteResolver leftDressArmResolver;
    public SpriteResolver rightDressArmResolver;
    public SpriteResolver leftDressLegResolver;
    public SpriteResolver rightDressLegResolver;

    public SpriteResolver headResolver;
    public SpriteResolver torsoResolver;
    public SpriteResolver leftArmResolver;
    public SpriteResolver rightArmResolver;
    public SpriteResolver leftLegResolver;
    public SpriteResolver rightLegResolver;

    private CharacterData currentData;

    private int lCurrentSlot = -1;

    private int normalOrderInLayer = 23;
    private int coverOrderInLayer = 14;

    private void Start()
    {
        currentData = new CharacterData();
        instance = this;
    }


    public void SetHat(string label)
    {
        currentData.hat = label;
        hatResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.Hat.ToString(), label);

        //poseControler?.PlayOneShotPose(PoseEnum.Joy,ReactionTime);
        poseControler?.SetFaceExpressionOneShot(ExpresionEnum.Joy, ReactionTime);
    }

    public void SetTorso(string label)
    {
        currentData.torso = label;

        bodyDressResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.Torso_Clothe.ToString(), label);
        leftDressArmResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.LeftArm_Clothe.ToString(), label);
        rightDressArmResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.RightArm_Clothe.ToString(), label);

        //poseControler?.PlayOneShotPose(PoseEnum.Joy,ReactionTime);
        poseControler?.SetFaceExpressionOneShot(ExpresionEnum.Joy, ReactionTime);
        LegOrdreManagement(label);
    }

    public void SetPant(string label)
    {
        currentData.pant = label;

        leftDressLegResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.LeftLeg_Clothe.ToString(), label);
        rightDressLegResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.RightLeg_Clothe.ToString(), label);

        //poseControler?.PlayOneShotPose(PoseEnum.Joy,ReactionTime);
        poseControler?.SetFaceExpressionOneShot(ExpresionEnum.Joy, ReactionTime);
    }

    public void SetSkin(string label)
    {
        currentData.pant = label;

        headResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.Head.ToString(), label);
        torsoResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.Torso.ToString(), label);
        rightLegResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.RightLeg.ToString(), label);
        leftLegResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.LeftLeg.ToString(), label);
        rightArmResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.RightArm.ToString(), label);
        leftArmResolver.SetCategoryAndLabel(BoddyPartLabelTypeEnum.LeftArm.ToString(), label);

        poseControler?.SetFaceExpressionOneShot(ExpresionEnum.Joy, ReactionTime);
        //poseControler?.PlayOneShotPose(PoseEnum.Joy,ReactionTime);
    }


    public void Randomize()
    {
        string torsoLabel;
        SetHat(Dressing.outfitLabels[Random.Range(0, Dressing.outfitLabels.Count)]);
        SetTorso(torsoLabel = Dressing.outfitLabels[Random.Range(0, Dressing.outfitLabels.Count)]);
        SetPant(Dressing.outfitLabels[Random.Range(0, Dressing.outfitLabels.Count)]);

        poseControler?.SetFaceExpressionOneShot(ExpresionEnum.Joy, ReactionTime);
        //poseControler?.PlayOneShotPose(PoseEnum.Joy,ReactionTime);

        LegOrdreManagement(torsoLabel);

    }


    public void SaveCharacter()
    {
        if (lCurrentSlot < 0)
        {
            Debug.LogWarning("Aucun slot sélectionné");
            return;
        }

        CharacterDatabase.instance.SaveCharacter(lCurrentSlot, currentData);

        poseControler?.SetFaceExpressionOneShot(ExpresionEnum.Joy, ReactionTime);
        poseControler?.PlayOneShotPose(PoseEnum.Joy, ReactionTime);
    }


    public void LoadCharacter(CharacterData data)
    {
        currentData = data;

        SetHat(data.hat);
        SetTorso(data.torso);
        SetPant(data.pant);

        poseControler?.PlayOneShotPose(PoseEnum.Joy, ReactionTime);

        //LegOrdreManagement();
    }

    public CharacterData GetCurrentData()
    {
        return currentData;
    }

    public string EressEmum2StringLabel(DressLabelTypeEnum pExpresion)
    {
        return pExpresion.ToString();
    }


    private void LegOrdreManagement(string pLabel)
    {
        if (!System.Enum.TryParse(pLabel, out DressLabelTypeEnum labelEnum))
        {
            Debug.LogWarning($"jannnabe: {pLabel}");
            legSprite.sortingOrder = normalOrderInLayer;
            return;
        }

        bool pCoverLeg = coverLegClothing.Contains(labelEnum);

        if (pCoverLeg)
        {
            legSprite.sortingOrder = coverOrderInLayer;
        }
        else
        {
            legSprite.sortingOrder = normalOrderInLayer;
        }
    }

    public void SelectSlot(int pSlotIndex)
    {
        lCurrentSlot = pSlotIndex;

        CharacterData lData = CharacterDatabase.instance.LoadCharacter(pSlotIndex);

        if (lData != null)
        {
            LoadCharacter(lData);
        }
        else
        {
            currentData = new CharacterData(); 
        }
    }
}