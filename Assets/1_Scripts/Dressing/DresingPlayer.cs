using UnityEngine;
using UnityEngine.U2D.Animation;


public class DresingPlayer : MonoBehaviour
{
    [SerializeField] private Transform cardContainer;
    [SerializeField] private DressingCard dressingCardFactory;
    [SerializeField] int playerId = 1;


    [Header("Resolvers")]
    public SpriteResolver headResolver;
    public SpriteResolver bodyResolver;
    public SpriteResolver LeftArmResolver;
    public SpriteResolver RightArmResolver;
    public SpriteResolver RightLegResolver;
    public SpriteResolver LeftLegsolver;

    public void CreateCards()
    {
        foreach (Transform child in cardContainer)
        {
            Destroy(child.gameObject);
        }

        CreateOneCard(DressType.Hat);
        CreateOneCard(DressType.Body);
        CreateOneCard(DressType.Pant);
    }

    void CreateOneCard(DressType type)
    {
        if (dressingCardFactory == null || cardContainer == null)
        {
            return;
        }

        DressingCard card = Instantiate(dressingCardFactory, cardContainer);

        string randomLabel = Dressing.outfitLabels[Random.Range(0, Dressing.outfitLabels.Count)];

        card.dressType = type;
        card.Costume = randomLabel;
        card.Player = this;

        string category = type switch
        {
            DressType.Hat => "Hat",
            DressType.Body => "Torso_Clothe",
            DressType.Pant => "RightLeg_Clothe",
            _ => ""
        };

        card.SetSprite(category, randomLabel);
    }




    public void ApplyPant(string pLabel)
    {
        RightLegResolver.SetCategoryAndLabel("RightLeg_Clothe", pLabel);
        LeftLegsolver.SetCategoryAndLabel("LeftLeg_Clothe", pLabel);

        checkWin();
    }

    public void ApplyTorso(string pRandomLabel)
    {

        bodyResolver.SetCategoryAndLabel("Torso_Clothe", pRandomLabel);
        RightArmResolver.SetCategoryAndLabel("RightArm_Clothe", pRandomLabel);
        LeftArmResolver.SetCategoryAndLabel("LeftArm_Clothe", pRandomLabel);

        checkWin();
    }

    public void ApplyHat(string pRandomLabel)
    {
        headResolver.SetCategoryAndLabel("Hat", pRandomLabel);

        checkWin();
    }


    private void checkWin()
    {

        string hatLabel = headResolver.GetLabel();
        string torsoLabel = bodyResolver.GetLabel();
        string pantLabel = RightLegResolver.GetLabel();


        if (hatLabel == Dressing.modelLabel && torsoLabel == Dressing.modelLabel && pantLabel == Dressing.modelLabel)
        {
            GameManager.instance.WinGame(GameManager.GetInstance().GetPlayerInList(playerId));
        }
    }
}
