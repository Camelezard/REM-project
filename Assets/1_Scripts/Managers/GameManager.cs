using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    private const int MAX_PLAYER = 2;

    private List<Player> _PlayersList;

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
        _PlayersList.Clear();

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
        
    }




    //
}