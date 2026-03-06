using System;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    public static event Action<int> OnBallPass;

    [SerializeField] private PlayerZone _PlayerZone;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("FlipperBall"))
        {
            if (_PlayerZone == PlayerZone.Top)
            {
                OnBallPass.Invoke((int)PlayerZone.Top);
            }

            else if (_PlayerZone == PlayerZone.Bottom)
            {
                OnBallPass.Invoke((int)PlayerZone.Bottom);
            }
        }
    }
}
