using UnityEngine;
using UnityEngine.U2D.Animation;
using UnityEngine.UI;

public class CustomizeButton : MonoBehaviour
{
    [SerializeField] public DressLabelTypeEnum dressLabelType = DressLabelTypeEnum.REM_Clothesv2;
    [SerializeField] public DressType dressType = DressType.Hat;

    [SerializeField] private Image buttonImage;
    [SerializeField] private SpriteLibraryAsset spriteLibrary;

    public void OnCardClicked()
    {
        ChangeCharacter();
    }

    private void Start()
    {
        UpdateImage();
    }

    private void ChangeCharacter()
    {
        string DressLabel = dressLabelType.ToString();
        switch (dressType)
        {
            case DressType.Pant:
                CharacterCustomizer.instance.SetPant(DressLabel);
                break;

            case DressType.Body:
                CharacterCustomizer.instance.SetTorso(DressLabel);
                break;


            case DressType.Hat:
                CharacterCustomizer.instance.SetHat(DressLabel);
                break;


            case DressType.Skin:
                CharacterCustomizer.instance.SetSkin(DressLabel);
                break;
        }
    }

    private void UpdateImage()
    {
        string category = GetCategory();
        string label = dressLabelType.ToString();

        Sprite sprite = spriteLibrary.GetSprite(category, label);

        if (sprite != null)
        {
            buttonImage.sprite = sprite;
        }
        else Destroy(gameObject);
    }

    private string GetCategory()
    {
        switch (dressType)
        {
            case DressType.Hat:
                return BoddyPartLabelTypeEnum.Hat.ToString();

            case DressType.Body:
                return BoddyPartLabelTypeEnum.Torso_Clothe.ToString();

            case DressType.Pant:
                return BoddyPartLabelTypeEnum.RightLeg_Clothe.ToString();

            case DressType.Skin:
                return BoddyPartLabelTypeEnum.Head.ToString();

            default:
                return "";
        }
    }
}
