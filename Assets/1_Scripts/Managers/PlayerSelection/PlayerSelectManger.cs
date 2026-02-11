using System.Collections.Generic;
using UnityEngine;

public class PlayerSelectManger : MonoBehaviour
{
    private List<Player> _PlayersRedy;
    void Start()
    {
        
    }

    public void StartGame()
    {
        GameManager.GetInstance().CreatePlayers(_PlayersRedy);
    }

    public void AddPlayer(Player lPlayer)
    {
        if (lPlayer == null || _PlayersRedy.Contains(lPlayer))
            return;

        _PlayersRedy.Add(lPlayer);

        Debug.Log("new player");
    }

    public void RemovePlayer(Player lPlayer)
    {
        if (lPlayer == null || !_PlayersRedy.Contains(lPlayer))
            return;

        _PlayersRedy.Remove(lPlayer);

        Debug.Log("player remove");
    }
}
