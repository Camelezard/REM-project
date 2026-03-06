using UnityEngine;

public class FlipperMinigameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    [SerializeField] private int _PointToWin = 3;

    private int[] _PlayerPoints;
    void Start()
    {
        _PlayerPoints = new int[GameManager.GetInstance().GetPlayersCount()];

        DeathZone.OnBallPass += AddPointTo;
    }

    private void AddPointTo(int pPLayer)
    {
        for (int i = 0; i < _PlayerPoints.Length; i++)
        {
            if (pPLayer == i)
            {
                _PlayerPoints[i]++;
                break;
            }
        }

        CheckIfWin();
    }

    private void CheckIfWin()
    {
        for (int i = 0; i < _PlayerPoints.Length; i++)
        {
            if (_PlayerPoints[i] >= _PointToWin)
            {
                break;
            }
        }
    }
}
