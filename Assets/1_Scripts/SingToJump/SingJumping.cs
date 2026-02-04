using UnityEngine;

public class SingJumping : MonoBehaviour
{
    public float velocityY = 0f;
    public float gravity = 9.81f;

    [HideInInspector] public bool isFalling = true;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
    }

    private void Gravity()
    {
        if (!isFalling) return;

        velocityY -= Time.deltaTime * gravity;
        if (velocityY >= gravity)
        {
            velocityY = gravity;
        }

        transform.position += Vector3.up * velocityY * Time.deltaTime;
    }
}
