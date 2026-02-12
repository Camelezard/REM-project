using System.Collections.Generic;
using UnityEditor.ShaderGraph;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
    [SerializeField] private Pawn _PawnFactory;
    public List<Pawn> _PawnList;

    public void SpawnPawns()
    {
        ClearPawns();
        Pawn lPawn;

        foreach (Player pPlayer in GameManager.GetInstance()._PlayersList)
        {
            CreateAPawn(out lPawn);
            _PawnList.Add(lPawn);
        }
    }


    private void ClearPawns()
    {
        if(_PawnList == null || _PawnList.Count <= 0)
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
        lPawn = Instantiate(_PawnFactory);

        if (lPawn == null)
        {
            Debug.Log ("no _pawnFactory refered");
            return;
        }
    }
}
