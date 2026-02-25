using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public enum TypOfMinigame
{
    Random,
    TugOFWar,
    SingToJump,
    Memory,
    Dressing,
    Dancing
}


public class GameManager : MonoBehaviour
{
    //private const string MAIN_BOARD_SCENE_NAME = "MainBoard";
    private const string MAIN_BOARD_SCENE_NAME = "MainBoard";
    private const string MINIGAME_TEST = "Memory";

    private const string MINIGAME_TUG_OF_WAR = "Tir a la corde";
    private const string MINIGAME_SHOOTING = "ShootingMinigame";
    private const string MINIGAME_SING_TO_JUMP = "SingToJump";
    private const string MINIGAME_MEMORY = "Memory";
    private const string MINIGAME_DRESSING = "Habillage";
    private const string MINIGAME_DANSING = "RetenirLesGestes";

    private string CurrentMinigameName;

    public static GameManager instance { get; private set; }
    public List<Player> _PlayersList { get; private set; }

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
        //instance = null;
    }

    public void CreatePlayers(List<Player> lPlayerNumber)
    {
        _PlayersList = new List<Player>(lPlayerNumber);
    }


    //Get turn
    public Player GetCurrentPlayerTurn() => _PlayersList[CurrentPlayer];
    public Player GetPlayerOne() => _PlayersList[0];
    public Player GetPlayerTwo() => _PlayersList[1];
    public Player GetPlayerInList(int pIndex) => _PlayersList[pIndex - 1];
    public int GetCurrentPlayerIndex() => CurrentPlayer;



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
        SceneTransitionanager.instance.SwitchOverlay("MainMenuTest", MAIN_BOARD_SCENE_NAME);
    }

    public void StartMinigame(TypOfMinigame pType)
    {
        CurrentMinigameName = GetSceneNamWithEnum(pType);

        SceneManager.LoadScene(CurrentMinigameName, LoadSceneMode.Additive);

        Scene minigame = SceneManager.GetSceneByName(CurrentMinigameName);
        SceneManager.SetActiveScene(minigame);
    }

    public void WinGame(Player pPlayer)
    {
        if (CurrentMinigameName == null)
        {
            Debug.Log("CurrentMinigameName not set");
            return;
        }

        SceneManager.UnloadSceneAsync(CurrentMinigameName);
        CurrentMinigameName = null;

        Scene board = SceneManager.GetSceneByName(MAIN_BOARD_SCENE_NAME);
        SceneManager.SetActiveScene(board);

        NextPlayerTurn();
    }

    // private void ResetGameState()
    // {
    //     _PlayersList = new List<Player>();

    //     Debug.Log("Game state reset.");
    // }

    private string GetSceneNamWithEnum(TypOfMinigame pType)
    {
        string SceneName = MINIGAME_TEST;

        switch (pType)
        {
            case TypOfMinigame.TugOFWar:
                SceneName = MINIGAME_TEST;
                break;

            case TypOfMinigame.SingToJump:
                SceneName = MINIGAME_SING_TO_JUMP;
                break;

            case TypOfMinigame.Memory:
                SceneName = MINIGAME_MEMORY;
                break;

            case TypOfMinigame.Dressing:
                SceneName = MINIGAME_DRESSING;
                break;


            case TypOfMinigame.Dancing:
                SceneName = MINIGAME_DANSING;
                break;


            default:
                SceneName = MINIGAME_TEST;
                break;
        }

        return SceneName;
    }
}