using UnityEngine;
using UnityEngine.UI;

public class BoddyCloth : MonoBehaviour
{
    [SerializeField] private Dressing _Dressing;


    [Header("Cloth")]
    [SerializeField] private Image _Hat;
    [SerializeField] private Image _DerssTop;
    [SerializeField] private Image _Pent;
    [SerializeField] private Image _Shoes;

    [Header("Colors")]
    private Color _HideColor = new Color(1, 1, 1, 0);
    private Color _ShowColor = new Color(1, 1, 1, 1);



    void Start()
    {
        if (_Hat)
            ResetPartOfTheBody(_Hat);
        if (_DerssTop)
            ResetPartOfTheBody(_DerssTop);
        if (_Pent)
            ResetPartOfTheBody(_Pent);
        if (_Shoes)
            ResetPartOfTheBody(_Shoes);


    }

    void Update()
    {

    }

    private void ResetPartOfTheBody(Image lBoddyPartToReset)
    {
        lBoddyPartToReset.sprite = null;
        lBoddyPartToReset.color = _HideColor;
    }

    private void SetPartOfTheBody(Image lBoddyPartToSet,Sprite lCloth)
    {
        lBoddyPartToSet.sprite = null;
        lBoddyPartToSet.color = _ShowColor;
    }
}
