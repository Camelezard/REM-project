using NUnit.Framework;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class FlipperMinigameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int _PointToWin = 3;
    [SerializeField] private TextMeshProUGUI[] _ScoresTexts = new TextMeshProUGUI[2];
    [SerializeField] private GameObject _BallPrefab;

    private List<int> _PlayerPoints = new List<int>();
    void OnEnable()
    {
        //_PlayerPoints = new int[GameManager.GetInstance().GetPlayersCount()];
        _PlayerPoints = new List<int> { 0, 0 };

        DeathZone.OnBallPass += AddPointTo;

        SpawnBall();
    }

    private void AddPointTo(int pPLayer)
    {
        _PlayerPoints[pPLayer]++;
        Debug.Log(_PlayerPoints[pPLayer])   ;
        _ScoresTexts[pPLayer].text = _PlayerPoints[pPLayer].ToString();

        CheckIfWin(pPLayer);
        SpawnBall();
    }

    private void CheckIfWin(int pPlayer)
    { 
        if (_PlayerPoints[pPlayer] >= _PointToWin)
        {
            Debug.Log("hfhhfjskhjf");
            GameManager.GetInstance().WinGame(GameManager.GetInstance().GetPlayerInList(pPlayer));
        }
    }

    private void SpawnBall()
    {
        Vector2 lRandPos = new Vector2(Random.value*1.5f, Random.value*1.5f);
        Instantiate(_BallPrefab, lRandPos, Quaternion.identity);
    }

    private void OnDisable()
    {
        DeathZone.OnBallPass -= AddPointTo;
    }
}
