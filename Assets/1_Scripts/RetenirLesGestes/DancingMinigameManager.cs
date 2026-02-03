using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DancingMinigameManager : MonoBehaviour
{
    private List<int> _NumberSequence = new List<int>();
    [SerializeField] private List<Button> _Player1Buttons = new List<Button>();
    [SerializeField] private List<Button> _Player2Buttons = new List<Button>();
    public int currentLevel = 1;
    public int numberOfLevel = 5;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SetupButtons();
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitGame()
    {
        currentLevel = 1;
        _NumberSequence.Clear();
        GenerateSequenceOfNumber();
    }

    private void SetupButtons()
    {
        for (int i = 0; i < _Player1Buttons.Count; i++)
        {
            _Player1Buttons[i].onClick.AddListener(() => OnButtonPress(1, i + 1));
        }

        for (int i = 0; i < _Player2Buttons.Count; i++)
        {
            _Player2Buttons[i].onClick.AddListener(() => OnButtonPress(2, i + 1));
        }
    }

    private void GenerateSequenceOfNumber()
    {
        for (int i = 0; i < numberOfLevel; i++)
        {
            _NumberSequence.Add(Random.Range(1, 4)); ;
        }
        
    }

    private IEnumerator DisplaySequence()
    {
        yield return null;
    }

    private void OnButtonPress(int pPlayer, int pButtonValue)
    {
        Debug.Log(pPlayer + "Pressed : " +  pButtonValue);
    }
}
