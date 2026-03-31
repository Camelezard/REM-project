using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase instance;

    [SerializeField] public int maxSlots = 3;

    public List<CharacterData> characters = new List<CharacterData>();

    private void Awake()
    {
        instance = this;

        LoadFromDisk();

        if (characters == null)
            characters = new List<CharacterData>();

        while (characters.Count < maxSlots)
        {
            characters.Add(null);
        }
    }

    public void SaveCharacter(int pSlotIndex, CharacterData pData)
    {
        if (pSlotIndex < 0 || pSlotIndex >= characters.Count)
        {
            Debug.LogError("Slot invalide");
            return;
        }

        characters[pSlotIndex] = pData;

        SaveToDisk();
    }

    public void SaveToDisk()
    {
        CharacterDataListWrapper lWrapper = new CharacterDataListWrapper();
        lWrapper.list = characters;

        string lJson = JsonUtility.ToJson(lWrapper);

        PlayerPrefs.SetString("CHARACTERS", lJson);
        PlayerPrefs.Save();
    }



    public CharacterData LoadCharacter(int pSlotIndex)
    {
        if (pSlotIndex < 0 || pSlotIndex >= characters.Count)
        {
            Debug.LogError($"Slot invalide: {pSlotIndex} / {characters.Count}");
            return null;
        }

        return characters[pSlotIndex];
    }

    public void LoadFromDisk()
    {
        if (!PlayerPrefs.HasKey("CHARACTERS"))
            return;

        string lJson = PlayerPrefs.GetString("CHARACTERS");

        CharacterDataListWrapper lWrapper = JsonUtility.FromJson<CharacterDataListWrapper>(lJson);

        characters = lWrapper.list;

        if (characters == null)
            characters = new List<CharacterData>();

        while (characters.Count < maxSlots)
        {
            characters.Add(null);
        }
    }




    public bool HasCharacter(int pSlotIndex)
    {
        return characters[pSlotIndex] != null;
    }

    public int GetMaxSlots()
    {
        return maxSlots;
    }

    public List<CharacterData> GetAllSavedCharacters()
    {
        List<CharacterData> list = new List<CharacterData>();

        for (int i = 0; i < maxSlots; i++)
        {
            if (characters[i] != null)
            {
                list.Add(characters[i]);
            }
        }

        return list;
    }
}