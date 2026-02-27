using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.UI;

public class BoardTile : MonoBehaviour
{

    private enum TileType
    {
        StartTile,
        GameTile,
        RandoGameTile,
        EffectTile,
        EndTile
    }

    public SplineContainer _TileSplineContainer;
    private TileType tileType;

    [Header("Tile Specifications")]
    [Range(0f, 1f)] public float distanceOnPath;
    [SerializeField] public Spline spline;
    [SerializeField] private MeshRenderer _MeshRend;
    [SerializeField] private Text _TileText;
    [SerializeField] private bool _CanResetColor = true;
    [SerializeField] private TextMeshPro _EffectBillBoard;

    [SerializeField] private bool _UpdateVisual = false;

    public int tileIndex = 0;

    private List<TileEffect> _Effects = new List<TileEffect>();

    private void Awake()
    {
        _Effects.AddRange(GetComponents<TileEffect>());
    }

    void OnValidate()
    {
        AdjustOnSline();

        _Effects.Clear();
        _Effects.AddRange(GetComponents<TileEffect>());
        AdjustEffectBillBoardText();

        string lDescription;
        foreach (TileEffect lEffect in _Effects)
        {
            lDescription = "- " + lEffect.GetDescription() + "\n";
        }
        _UpdateVisual = false;
        if (_CanResetColor)
        {
            ResetColor();
        }
    }

    /// <summary>
    /// 
    /// permet d'ajuster la position de la tile pour les raprocher ou les ecarter
    /// <summary>
    public void AdjustOnSline()
    {
        if (_TileSplineContainer == null)
        {
            Debug.Log("_TileSplineContainer not assigned");
            return;
        }

        transform.position = _TileSplineContainer.EvaluatePosition(distanceOnPath);
    }

    public bool LaunchTileEffect(Pawn pPawn)
    {
        if (_Effects.Count == 0) return false;
        else
        {
            foreach (TileEffect lEffect in _Effects)
            {
                lEffect.Execute(pPawn);
            }

            return true;
        }
    }


    // Colors

    public void ChangeColor(Material pTileColor)
    {
        if (pTileColor == null || _MeshRend == null) return;

        var mats = _MeshRend.sharedMaterials;

        if (mats.Length > 1)
        {
            mats[2] = pTileColor;
            _MeshRend.sharedMaterials = mats;
        }
        print("change");
    }


    public void ResetColor()
    {
        ChangeColor(UnityEditor.AssetDatabase.LoadAssetAtPath<Material>("Assets/Materials/TileEfectMaterials/NullTileEffectColor.mat"));
        _CanResetColor = false;

        print("colorReset");
    }

    public void ChangeText(int pNum)
    {
        _TileText.text = pNum.ToString();
    }

    private void AdjustEffectBillBoardText()
    {
        if (_Effects.Count == 0) _EffectBillBoard.gameObject.SetActive(false);
        else
        {
            _EffectBillBoard.gameObject.SetActive(true);
            for (int i = 0; i < _Effects.Count; i++)
            {
                _EffectBillBoard.text = _Effects[i].billboardEffectMessage; 
            }
        }
    }

    private void ShowEffectBillBoard()
    {
        if (BoardManager.Instance.GetCurrentPawn().currentTile + 6 <= tileIndex)
        {
            _EffectBillBoard.gameObject.SetActive(true);
        }
        else
        {
            _EffectBillBoard.gameObject.SetActive(false);
        }
    }
}
