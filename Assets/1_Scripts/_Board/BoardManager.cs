using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using System;

public class BoardManager : MonoBehaviour
{
    public static BoardManager Instance;

    public static Action OnPlayerWin;
    public static Action OnNextTurn;
    public static Action OnFinishPawnsSpawn;
    public static Action OnPlayerFinishTurn;
    public static event Action OnPlayerAboutToMove;
    public static Action OnpLplayerFinshTun;
    public static Action OnMinigameFinished;

    [SerializeField] private Pawn _PawnFactory;
    [SerializeField] public SplineContainer _SplineContainer;
    [SerializeField] private float _SpawnTime = 1.5f;
    [SerializeField] private float _PlayerTransitionTime = 2f;

    public List<Pawn> pawnList { get; private set; } = new List<Pawn>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }
    }

    void Start()
    {

    }


    public Pawn GetCurrentPawn(int index) => pawnList[index];
    public Pawn GetCurrentPawn() => pawnList[GameManager.GetInstance().GetCurrentPlayerIndex()];

    public Pawn GetPawnWithPlayer(Player pPlayer)
    {
        return pawnList[pPlayer.playerId];
    }

    //------------- Events  ---------------------
    void OnEnable()
    {
        SceneTransitionanager.OnSceneReadyFirstTime += OnFirstLoadStartTransition;
        OnNextTurn += LunchPlayerFocusTransition;
        OnFinishPawnsSpawn += LunchPlayerFocusTransition;
    }


    private void OnDisable()
    {
        SceneTransitionanager.OnSceneReadyFirstTime -= OnFirstLoadStartTransition;
        OnNextTurn -= LunchPlayerFocusTransition;
        OnFinishPawnsSpawn -= LunchPlayerFocusTransition;

    }




    //---------------   PawnsCreation   ---------------
    public IEnumerator SpawnPawns()
    {
        ClearPawns();
        Pawn lPawn;
        List<Player> lPlayers = GameManager.GetInstance()._PlayersList;
        float lWaitTime = _SpawnTime / lPlayers.Count;

        foreach (Player pPlayer in GameManager.GetInstance()._PlayersList)
        {
            CreateAPawn(out lPawn);
            lPawn._SplineContainer = _SplineContainer;
            pawnList.Add(lPawn);

            yield return new WaitForSeconds(lWaitTime);
        }

        OnFinishPawnsSpawn?.Invoke();
    }


    private void ClearPawns()
    {
        if (pawnList == null || pawnList.Count <= 0)
        {
            return;
        }

        foreach (Pawn lPawn in pawnList)
        {
            Destroy(lPawn.gameObject);
        }

        pawnList.Clear();

    }

    private void CreateAPawn(out Pawn lPawn)
    {
        lPawn = null;

        if (_PawnFactory == null)
        {
            Debug.LogError("No PawnFactory assigned");
            return;
        }

        lPawn = Instantiate(_PawnFactory);

        Debug.Log("PawnCrated");

    }



    //---------------   Transition   ---------------
    public void LunchPlayerFocusTransition()
    {
        StartCoroutine(FocusPlayer());
    }

    private IEnumerator FocusPlayer()
    {
        OnPlayerAboutToMove.Invoke();

        yield return new WaitForSeconds(2f);

        Pawn lPawn = GetCurrentPawn();

        CameraManager.Instance.UpdateTarget(lPawn.transform);

        yield return new WaitForSeconds(_PlayerTransitionTime);

        lPawn.StartTurn();
    }

    public void OnFirstLoadStartTransition()
    {
        StartCoroutine(SpawnPawns());

        Debug.Log("StartTransition");

    }
}
