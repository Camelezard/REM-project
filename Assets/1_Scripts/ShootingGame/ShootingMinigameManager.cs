using TMPro;
using UnityEngine;

public class ShootingMinigameManager : MonoBehaviour
{
    public float gameDuration = 45f;
    private float _ElapsedTime = 45f;

    public TextMeshProUGUI timerText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _ElapsedTime = gameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        _ElapsedTime -= Time.deltaTime;
        timerText.text = _ElapsedTime.ToString("F0");
    }
}
