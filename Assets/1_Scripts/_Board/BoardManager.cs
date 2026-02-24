using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Splines;
using System;

public class BoardManager : MonoBehaviour
{
    public static Action OnPlayerWin;
    public static Action OnNextTurn;
    public static Action OnFinishPawnsSpawn;
    public static Action OnpLplayerFinshTun;

    [SerializeField] private Pawn _PawnFactory;
    [SerializeField] public SplineContainer _SplineContainer;
    [SerializeField] private float _SpawnTime = 1.5f;
    [SerializeField] private float _PlayerTransitionTime = 2f;

    private List<Pawn> _PawnList = new List<Pawn>();


    void Start()
    {

    }


    public Pawn GetCurrentPawn(int index) => _PawnList[index];
    public Pawn GetCurrentPawn() => _PawnList[GameManager.GetInstance().GetCurrentPlayerIndex()];

    //------------- Events  ---------------------
    void OnEnable()
    {
        SceneTransitionanager.OnSceneReadyFirstTime += OnFirstLoadStartTransition;
        OnNextTurn += LunchSpawnPlayerFocusTransition;
        OnFinishPawnsSpawn += LunchSpawnPlayerFocusTransition;
    }


    private void OnDisable()
    {
        SceneTransitionanager.OnSceneReadyFirstTime -= OnFirstLoadStartTransition;
        OnNextTurn -= LunchSpawnPlayerFocusTransition;
        OnFinishPawnsSpawn -= LunchSpawnPlayerFocusTransition;
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
            _PawnList.Add(lPawn);

            yield return new WaitForSeconds(lWaitTime);
        }

        OnFinishPawnsSpawn?.Invoke();
    }


    private void ClearPawns()
    {
        if (_PawnList == null || _PawnList.Count <= 0)
        {
            return;
        }

        foreach (Pawn lPawn in _PawnList)
        {
            Destroy(lPawn.gameObject);
        }

        _PawnList.Clear();

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
    public void LunchSpawnPlayerFocusTransition()
    {
        StartCoroutine(FocusPlayer());
    }

    private IEnumerator FocusPlayer()
    {
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
