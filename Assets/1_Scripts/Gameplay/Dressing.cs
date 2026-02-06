using System.Collections.Generic;
using UnityEngine;

public class Dressing : MonoBehaviour
{
    [Header("Closes")]
    [SerializeField] private List<Sprite> _Pants;
    [SerializeField] private List<Sprite> _Choses;
    [SerializeField] private List<Sprite> _DressTop;

    public List<Sprite> pants => _Pants;
    public List<Sprite> choses => _Choses;
    public List<Sprite> dressTop => _DressTop;

}
