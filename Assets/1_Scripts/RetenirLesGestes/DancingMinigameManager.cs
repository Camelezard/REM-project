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
    private List<int> _Player1Inputs = new List<int>();
    private List<int> _Player2Inputs = new List<int>();
    private int _Player1CurrentIndex = 0;
    private int _Player2CurrentIndex = 0;
    public int currentLevel = 1;
    public int numberOfLevel = 5;

    private bool _IsPlayersTurn = false;
    private bool _IsDisplayingSequence = false;
    private Coroutine _DisplaySequenceCoroutine;

    public TextMeshProUGUI displayText;
    
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
        _DisplaySequenceCoroutine = StartCoroutine(DisplaySequence());
    }

    private void SetupButtons()
    {
       
        for (int i = 0; i < _Player1Buttons.Count; i++)
        {
            int lButtonValue = i + 1;
            _Player1Buttons[i].onClick.AddListener(() => OnButtonPress(1, lButtonValue));
        }

        for (int i = 0; i < _Player2Buttons.Count; i++)
        {
            int lButtonValue = i + 1;
            _Player2Buttons[i].onClick.AddListener(() => OnButtonPress(2, lButtonValue));
        }
    }

    private void GenerateSequenceOfNumber()
    {
        for (int i = 0; i < numberOfLevel; i++)
        {
            _NumberSequence.Add(Random.Range(1, 4));
            Debug.Log(_NumberSequence[i]);
        }
        
    }

    private IEnumerator DisplaySequence()
    {
        _IsDisplayingSequence = true;
        _IsPlayersTurn = false;
        SetButtonsInteractable(1, false);
        SetButtonsInteractable(2, false);

        displayText.text = "Ready ?";
        yield return new WaitForSeconds(2f);

        for (int i = 0; i < currentLevel; i++)
        {
            displayText.text = _NumberSequence[i].ToString();
            yield return new WaitForSeconds(1);

            displayText.text = "";
            yield return new WaitForSeconds(0.5f);
        }

        _IsDisplayingSequence = false;
        _IsPlayersTurn = true;
        SetButtonsInteractable(1, true);
        SetButtonsInteractable(2, true);

        StopCoroutine(_DisplaySequenceCoroutine);
    }

    private void OnButtonPress(int pPlayer, int pButtonValue)
    {
        if (!_IsPlayersTurn) return;

        if (pPlayer == 1)
        {
            _Player1Inputs.Add(pButtonValue);

            if (_Player1Inputs[_Player1CurrentIndex] == _NumberSequence[_Player1CurrentIndex])
            {
                if (_Player1CurrentIndex + 1 >= currentLevel) SetButtonsInteractable(1, false);
                _Player1CurrentIndex++;
            }
            else
            {
                displayText.fontSize = 300;
                displayText.text = "Player 1 lose...";
            }
        }

        if (pPlayer == 2)
        {
            _Player2Inputs.Add(pButtonValue);

            if (_Player2Inputs[_Player2CurrentIndex] == _NumberSequence[_Player2CurrentIndex])
            {
                if (_Player2CurrentIndex + 1 >= currentLevel) SetButtonsInteractable(2, false);
                _Player2CurrentIndex++;
            }
            else
            {
                displayText.fontSize = 300;
                displayText.text = "Player 2 lose...";
            }
        }

        CheckEndOfMinigame();
    }

    private void CheckEndOfMinigame()
    {
        if (_Player1CurrentIndex >= currentLevel && _Player2CurrentIndex >= currentLevel)
        {
            _Player1CurrentIndex = 0;
            _Player2CurrentIndex = 0;
            _Player1Inputs.Clear();
            _Player2Inputs.Clear();

            _IsPlayersTurn = false;
            currentLevel++;
            Debug.Log(currentLevel);

            if (_DisplaySequenceCoroutine != null)
            {
                StopCoroutine(_DisplaySequenceCoroutine);
                _DisplaySequenceCoroutine = null;
            }

            if (currentLevel > numberOfLevel)
            {
                displayText.text = "COOL !";
            }
            else _DisplaySequenceCoroutine = StartCoroutine(DisplaySequence());
        }
    }

    private void SetButtonsInteractable(int pPlayer,bool pInteractable)
    {
        if (pPlayer == 1)
        {
            foreach (Button button in _Player1Buttons)
            {
                button.interactable = pInteractable;
            }
        }
        
        if (pPlayer == 2)
        {
            foreach (Button button in _Player2Buttons)
            {
                button.interactable = pInteractable;
            }
        }
    }
}
