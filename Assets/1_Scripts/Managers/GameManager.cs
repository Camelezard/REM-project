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
    private const string MAIN_BOARD_SCENE_NAME_TEST = "MainBoardTest";
    private const string MINIGAME_TEST = "Memory";

    private const string MINIGAME_TUG_OF_WAR = "Tir a la corde";
    private const string MINIGAME_SHOOTING = "ShootingMinigame";
    private const string MINIGAME_SING_TO_JUMP = "SingToJump";
    private const string MINIGAME_MEMORY = "Memory";
    private const string MINIGAME_DRESSING = "Habillage";
    private const string MINIGAME_DANSING = "RetenirLesGestes";

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
        instance = null;
    }

    public void CreatePlayers(List<Player> lPlayerNumber)
    {
        _PlayersList = new List<Player>(lPlayerNumber);
    }


    //Get turn
    public Player GetCurrentPlayerTurn() => _PlayersList[CurrentPlayer];
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
        SceneTransitionanager.instance.SwitchOverlay("MainMenuTest", MAIN_BOARD_SCENE_NAME_TEST);
    }

    public void StartMinigame(TypOfMinigame pTyp)
    {
        SceneTransitionanager.instance.LoadSingle(GetSceneNamWithEnum(pTyp));
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

    private string GetSceneNamWithEnum(TypOfMinigame pTyp)
    {
        string SceneName = MINIGAME_TEST;

        switch (pTyp)
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