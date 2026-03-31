using UnityEngine;

public class CharacterLoader : MonoBehaviour
{
    [SerializeField] private CharacterCustomizer _Customizer;

    [Header("Mode")]
    [SerializeField] private bool _UsePlayer = false;
    [SerializeField] private int _PlayerIndex = 0;

    [SerializeField] private bool _UseSlot = false;
    [SerializeField] private int _SlotIndex = 0;

    private Player _Player;

    void Awake()
    {
        if (_Customizer == null)
            _Customizer = GetComponent<CharacterCustomizer>();
    }

    void Start()
    {
        if (_UseSlot)
        {
            LoadFromSlot();
        }
        else if (_UsePlayer)
        {
            LoadFromPlayerIndex();
        }
    }

    public void Init(Player pPlayer)
    {
        _Player = pPlayer;
        Apply(_Player.characterData);
    }

    public void LoadFromSlot(int pSlot = -1)
    {
        CharacterData lData;

        if (pSlot < 0) lData = CharacterDatabase.instance.LoadCharacter(_SlotIndex);
        else lData = CharacterDatabase.instance.LoadCharacter(pSlot);

        if (lData == null)
            return;

        Apply(lData);
    }

    public void LoadFromPlayerIndex()
    {
        Player lPlayer = GameManager.GetInstance().GetPlayerInList(_PlayerIndex);

        if (lPlayer == null)
            return;

        Apply(lPlayer.characterData);
    }

    private void Apply(CharacterData pData)
    {
        if (_Customizer == null || pData == null)
            return;

        _Customizer.LoadCharacter(pData);
    }
}