using UnityEngine;
using System;

public class PlayerSelect : MonoBehaviour
{
    [SerializeField] private PlayerSelectManger _PlayerManager = null;
    [SerializeField] private int _SelectedSlot = -1;
    private Player _SelfPlayer = null;
    private bool _IsReady = false;


    void Start()
    {
        _SelfPlayer = new Player();

        if (_SelectedSlot >= 0) SelectCharacterSlot(_SelectedSlot);
    }

    public void OnRedyClick()
    {
        if (!_PlayerManager)
        {
            Debug.Log("_PlayerManager not refered");
            return;
        }

        if (!_IsReady)
        {
            _PlayerManager.AddPlayer(_SelfPlayer);
        }
        else
        {
            _PlayerManager.RemovePlayer(_SelfPlayer);
        }

        _IsReady = !_IsReady;
    }

    public void SelectCharacterSlot(int pSlotIndex)
    {
        _SelectedSlot = pSlotIndex;

        CharacterData lData = CharacterDatabase.instance.LoadCharacter(pSlotIndex);

        if (lData == null)
        {
            Debug.Log("Slot vide");
            return;
        }

        _SelfPlayer.characterData = lData;

        Debug.Log("Character assigned to player");
    }

    public void LoadAppearanceFromSlot(int slotIndex)
    {
        if (slotIndex < 0) return;

        CharacterLoader loader = GetComponent<CharacterLoader>();
        if (loader != null)
        {
            loader.LoadFromSlot(slotIndex); 
        }
    }
}
