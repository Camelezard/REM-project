using TMPro;
using UnityEngine;

public class ShootingMinigameManager : MonoBehaviour
{
    public float gameDuration = 45f;
    private float _ElapsedTime = 45f;

    public TextMeshProUGUI timerText;
    public Player shooterPlayer;
    public Player dodgerPlayer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ElapsedTime = gameDuration;
        shooterPlayer = GameManager.GetInstance().GetCurrentPlayerTurn();
        if(shooterPlayer != GameManager.GetInstance().GetPlayerOne())
        {
            dodgerPlayer = GameManager.GetInstance().GetPlayerOne();
        }
        else
        {
            dodgerPlayer = GameManager.GetInstance().GetPlayerTwo();
        }
    }

    // Update is called once per frame
    void Update()
    {
        _ElapsedTime -= Time.deltaTime;
        timerText.text = _ElapsedTime.ToString("F0");

        if (_ElapsedTime <= 0)
        {
            GameManager.GetInstance().WinGame(dodgerPlayer);
        }
    }
}
