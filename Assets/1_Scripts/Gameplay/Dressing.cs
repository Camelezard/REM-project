using System.Collections.Generic;
using UnityEngine;

public class Dressing : MonoBehaviour
{
    [SerializeField] ModelCreator Modele;
    public static List<string> outfitLabels = new List<string> { "Mousquetaire", "Clown", "Cowboy", "REM_Clothesv2" };

    public static string modelLabel;

    void Start()
    {
        InitializeModel();
    }

    void InitializeModel()
    {
        modelLabel = outfitLabels[Random.Range(0, outfitLabels.Count)];

        if (Modele == null) print("model not refered");
        else Modele.ApplyRandomOutfit(modelLabel);
    }
}
