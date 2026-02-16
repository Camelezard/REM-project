using System;
using System.Diagnostics;
using UnityEngine;

//public enum BoardSequanceState { Void, none, ShowBoard, ShowSomething, OnTransition }
//public enum TurnState { Void, FollowPlayer, ShowSomething, GoOnMinigame }

public class GameFlowManager : MonoBehaviour
{




    private static GameFlowManager instance;

    //private TurnState gameState;
    public static GameFlowManager GetInstance()
    {
        if (instance == null)
        {
            instance = FindFirstObjectByType<GameFlowManager>();

            if (instance == null)
            {
                GameObject lGameObject = new GameObject("FlowManager");
                instance = lGameObject.AddComponent<GameFlowManager>();
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

        //OnFinishPawnsSpawn += LunchSpawnPlayerTransition;
    }

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {

    }



    // Board Sequance
    public void LunchSpawnPlayerTransition()
    {
        
    }



    // Turn State
}
