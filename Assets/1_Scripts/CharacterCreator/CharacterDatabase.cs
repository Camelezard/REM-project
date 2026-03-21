using System.Collections.Generic;
using UnityEngine;

public class CharacterDatabase : MonoBehaviour
{
    public static CharacterDatabase instance;

    public List<CharacterData> characters = new List<CharacterData>();

    private const string SAVE_KEY = "CHARACTERS";

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void AddCharacter(CharacterData data)
    {
        characters.Add(data);
        Save();
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(new Wrapper { list = characters });
        PlayerPrefs.SetString(SAVE_KEY, json);
        PlayerPrefs.Save();
    }

    public void Load()
    {
        if (!PlayerPrefs.HasKey(SAVE_KEY)) return;

        string json = PlayerPrefs.GetString(SAVE_KEY);
        characters = JsonUtility.FromJson<Wrapper>(json).list;
    }

    public CharacterData GetCharacter(int index)
    {
        return characters[index];
    }

    [System.Serializable]
    private class Wrapper
    {
        public List<CharacterData> list;
    }
}