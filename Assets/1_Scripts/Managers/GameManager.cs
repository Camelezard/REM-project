using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class GameManager : MonoBehaviour
{

    //private const string MAIN_BOARD_SCENE_NAME = "MainBoard";
    private const string MAIN_BOARD_SCENE_NAME_TEST = "MainBoardTest";
    private const string MINIGAME_TEST = "MainBoardTest";

    public static GameManager instance { get; private set; }
    public List<Player> _PlayersList { get; private set; }
    public List<Pawn> _PawnList = new List<Pawn>();

    private int CurrentPlayer = 0;
    //private const int MAX_PLAYER = 2;


    // Get Instance
    public static GameManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<GameManager>();

            if (instance == null)
            {
                GameObject lGameObject = new GameObject("FlowManager");
                instance = lGameObject.AddComponent<GameManager>();
            }
        }

        return instance;
    }

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void OnDisable()
    {
        instance = null;
    }

    public void CreatePlayers(List<Player> lPlayerNumber)
    {
        _PlayersList = new List<Player>(lPlayerNumber);
    }


    //Get turn
    public Player GetCurrentPlayerTurn() => _PlayersList[CurrentPlayer];
    public Pawn GetCurrentPlayerTurnPawn() => _PawnList[CurrentPlayer];


    //Tunrn Management
    public void NextPlayerTurn()
    {
        SetNextPlayerTurnIndex();
        BoardManager.OnNextTurn.Invoke();
    }

    public void SetNextPlayerTurnIndex()
    {
        CurrentPlayer++;
        if (CurrentPlayer >= _PlayersList.Count) CurrentPlayer = 0;
    }


    // Scene Management
    public void LoadMainBoardFirstTime()
    {
        SceneTransitionanager.instance.SwitchOverlay("MainMenuTest", MAIN_BOARD_SCENE_NAME_TEST);
    }

    public void StartMinigame()
    {
        SceneTransitionanager.instance.LoadSingle(MINIGAME_TEST);
    }

    public void WinGame()
    {
        SceneTransitionanager.instance.LoadAdditive(MINIGAME_TEST);
    }

    // private void ResetGameState()
    // {
    //     _PlayersList = new List<Player>();

    //     Debug.Log("Game state reset.");
    // }

}