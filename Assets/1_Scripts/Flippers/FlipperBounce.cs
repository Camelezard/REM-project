using Unity.VisualScripting;
using UnityEngine;

public class FlipperBounce : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    [SerializeField] private float _BounceForceMultiplier = 1.5f;
    [SerializeField] private Rigidbody2D _FlipperRb;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("FlipperBall"))
        {
            Rigidbody2D lBallRb = collision.rigidbody;

            if (lBallRb != null)
            {
                Vector2 lNormal = collision.contacts[0].normal;
                Vector2 lIncomingVelocity = lBallRb.linearVelocity;

                Vector2 lFlipperVelocity = _FlipperRb.linearVelocity;

                Vector2 lBounceVelocity = Vector2.Reflect(lIncomingVelocity, -lNormal);
                lBallRb.linearVelocity = (lBounceVelocity + lFlipperVelocity) * _BounceForceMultiplier;
            }
        }
    }

}
