using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Pawn _PawnFactory;
    [SerializeField] public SplineContainer _SplineContainer;

    private List<Pawn> _PawnList;


    void Start()
    {
        SceneTransitionanager.OnSceneReady += OnStartTransition;
        _PawnList = GameManager.GetInstance()._PawnList;
    }

    public void OnStartTransition()
    {
        SpawnPawns();

        Debug.Log("StartTransition");

    }

    private void OnDisable()
    {
        SceneTransitionanager.OnSceneReady -= OnStartTransition;
    }

    public void SpawnPawns()
    {
        ClearPawns();
        Pawn lPawn;

        foreach (Player pPlayer in GameManager.GetInstance()._PlayersList)
        {
            CreateAPawn(out lPawn);
            lPawn._SplineContainer = _SplineContainer;
            _PawnList.Add(lPawn);
        }
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
}
