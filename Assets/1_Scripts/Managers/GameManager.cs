using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum TypOfMinigame
{
    Random,
    TugOFWar,
    Shooting,
    SingToJump,
    Memory,
    Dressing,
    Dancing,
    Flipper
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
    private const string MINIGAME_FLIPPER = "Flipper";

    private string CurrentMinigameName;
    private ScreenOrientation _MinigameOrientation;

    public static GameManager instance { get; private set; }
    public List<Player> _PlayersList { get; private set; }
    private List<TypOfMinigame> remainingMinigames = new List<TypOfMinigame>();

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


    void Start()
    {
        ResetRandLIstOfMinigames();
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

    public int GetPlayersCount()
    {
        return _PlayersList.Count;
    }


    // Scene Management
    public void LoadMainBoardFirstTime()
    {
        SceneTransitionanager.instance.SwitchOverlay("MainMenuTest", MAIN_BOARD_SCENE_NAME);
    }

    public void StartMinigame(TypOfMinigame pType)
    {
        StartCoroutine(LoadMinigameCoroutine(pType));
    }

    private IEnumerator LoadMinigameCoroutine(TypOfMinigame pType)
    {
        if (pType == TypOfMinigame.Random)
        {
            pType = GetRandomMinigame();
        }

        CurrentMinigameName = GetSceneNamWithEnum(pType);
        Screen.orientation = _MinigameOrientation;

        AsyncOperation op = SceneManager.LoadSceneAsync(CurrentMinigameName, LoadSceneMode.Additive);

        yield return op;

        Scene minigame = SceneManager.GetSceneByName(CurrentMinigameName);

        if (minigame.isLoaded)
        {
            SceneManager.SetActiveScene(minigame);
        }
        else
        {
            Debug.LogError("Scene not loaded properly");
        }
    }

    public void WinGame(Player pWiner, float pDelais = 1f)
    {
        StartCoroutine(WinGameCoroutine(pWiner,pDelais));
    }

    private IEnumerator WinGameCoroutine(Player pWiner, float pDelais = 1f)
    {
        yield return new WaitForSeconds(pDelais);

        if (CurrentMinigameName == null) yield break;

        SceneManager.UnloadSceneAsync(CurrentMinigameName);
        CurrentMinigameName = null;
        Screen.orientation = ScreenOrientation.LandscapeRight;

        Scene board = SceneManager.GetSceneByName(MAIN_BOARD_SCENE_NAME);
        SceneManager.SetActiveScene(board);

        BoardManager.OnMinigameFinished?.Invoke();

        if (pWiner == GetCurrentPlayerTurn())
        {
            BoardManager.OnNextTurn?.Invoke();
        }
        else
        {
            NextPlayerTurn();
        }
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
                _MinigameOrientation = ScreenOrientation.LandscapeRight;
                break;

            case TypOfMinigame.Shooting:
                SceneName = MINIGAME_SHOOTING;
                _MinigameOrientation = ScreenOrientation.Portrait;
                break;

            case TypOfMinigame.SingToJump:
                SceneName = MINIGAME_SING_TO_JUMP;
                _MinigameOrientation = ScreenOrientation.LandscapeRight;
                break;

            case TypOfMinigame.Memory:
                SceneName = MINIGAME_MEMORY;
                _MinigameOrientation = ScreenOrientation.LandscapeRight;
                break;

            case TypOfMinigame.Dressing:
                SceneName = MINIGAME_DRESSING;
                _MinigameOrientation = ScreenOrientation.LandscapeRight;
                break;


            case TypOfMinigame.Dancing:
                SceneName = MINIGAME_DANSING;
                _MinigameOrientation = ScreenOrientation.LandscapeRight;
                break;

            case TypOfMinigame.Flipper:
                SceneName = MINIGAME_FLIPPER;
                _MinigameOrientation = ScreenOrientation.Portrait;
                break;


            default:
                SceneName = MINIGAME_TEST;
                break;
        }

        return SceneName;
    }


    private void ResetRandLIstOfMinigames()
    {
        remainingMinigames = new List<TypOfMinigame>(
            (TypOfMinigame[])System.Enum.GetValues(typeof(TypOfMinigame))
        );

        remainingMinigames.Remove(TypOfMinigame.Random);

        Shuffle(remainingMinigames);
    }

    private void Shuffle(List<TypOfMinigame> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            int rand = Random.Range(i, list.Count);
            (list[i], list[rand]) = (list[rand], list[i]);
        }
    }

    public TypOfMinigame GetRandomMinigame()
    {
        if (remainingMinigames.Count == 0)
        {
            ResetRandLIstOfMinigames();
        }

        TypOfMinigame game = remainingMinigames[0];
        remainingMinigames.RemoveAt(0);

        return game;
    }
}