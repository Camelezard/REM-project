using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private const string MAIN_BOARD_SCENE_NAME = "MainBoard";
    private const string MAIN_BOARD_SCENE_NAME_TEST = "MainBoardTest";

    public static GameManager instance { get; private set; }
    public List<Player> _PlayersList {get; private set; }

    private const int MAX_PLAYER = 2;


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

    // Start
    void Start()
    {
        //CreatePlayers();
    }

    // fonctions utiles
    public void CreatePlayers(List<Player> lPlayerNumber)
    {
        _PlayersList = new List<Player>(lPlayerNumber);
        Player lPlayer;
        // for (int i = 0; i < lPlayerNumber; i++)
        // {
        //     lPlayer = new Player();

        //     _PlayersList.Add(lPlayer);

        //     lPlayer.playerId = i + 1;

        //     print ($"New player has been created id = {lPlayer.playerId}");
        // }
    }



    public void WinGame()
    {
        GoBackToMainBoard();
    }


    //Scene Management

    public void GoBackToMainBoardForFirstTime()
    {
        StartCoroutine(RsetMainBoard());
    }




    public void GoBackToMainBoard()
    {
        SceneManager.LoadScene(MAIN_BOARD_SCENE_NAME_TEST, LoadSceneMode.Additive);
    }

    private IEnumerator RsetMainBoard()
    {
        yield return SceneManager.UnloadSceneAsync(MAIN_BOARD_SCENE_NAME_TEST);
        yield return SceneManager.LoadSceneAsync(MAIN_BOARD_SCENE_NAME_TEST, LoadSceneMode.Additive);
    }

}