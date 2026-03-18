using UnityEngine;
using UnityEngine.UI;
using UnityEngine.U2D.Animation;
using Unity.VisualScripting;

public enum DressType { Pant, Hat, Body }

public class DressingCard : MonoBehaviour
{
    [SerializeField] public Image DresImage;
    public DressType dressType;
    public DresingPlayer Player;
    public string Costume;

    [SerializeField] private SpriteLibraryAsset spriteLibrary;


    public void SetSprite(string category, string label)
    {
        if (spriteLibrary == null)
        {
            spriteLibrary = GetComponent<SpriteLibraryAsset>();
        }

        if (spriteLibrary != null)
        {
            Sprite sprite = spriteLibrary.GetSprite(category, label);
            if (sprite != null)
                DresImage.sprite = sprite;
            else
                Debug.LogWarning($"pas de sprie {category}/{label}");
        }
        else
        {
            Debug.LogError("trouve pas la librairie");
        }
    }


    public void OnClicked()
    {
        switch (dressType)
        {
            case DressType.Pant:
            Player.ApplyPant(Costume);
            break;

            case DressType.Hat:
            Player.ApplyHat(Costume);
            break;

            case DressType.Body:
            Player.ApplyTorso(Costume);
            break;
        }
    }
}
