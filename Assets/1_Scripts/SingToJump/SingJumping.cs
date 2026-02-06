using Unity.VisualScripting;
using UnityEngine;

public class SingJumping : MonoBehaviour
{
    public float velocityY = 0f;
    public float gravity = 9.81f;

    public float minJumpForce = 0f;
    public float maxJumpForce = 100f;
    public AudioLoudnessDetection detector;

    public float loudnessSensibilty = 10;
    public float threshold = 0.1f;

    [HideInInspector] public bool isFalling = false;
    [HideInInspector] public bool isInAir = false;

    public RaycastCollision collision;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Gravity();
        JumpByLoudness();
        transform.position += Vector3.up * velocityY * Time.deltaTime;

        
    }

    private void Gravity()
    {
        if (!isFalling) return;

        velocityY -= Time.deltaTime * gravity;
        if (velocityY <= -gravity)
        {
            velocityY = -gravity;
        }

        if (velocityY <= 0) collision.isDeactivated = false;
        else collision.isDeactivated = true;
    }

    private void JumpByLoudness()
    {
        float lLoudness = detector.GetLoudnessFromMicrophone() * loudnessSensibilty;
        if (lLoudness < threshold) lLoudness = 0;

        
        lLoudness = Mathf.Clamp(lLoudness, minJumpForce, maxJumpForce);
        Debug.Log(lLoudness);
        if (!isInAir)
        {
            velocityY = 0;
            velocityY += lLoudness;
        }
        
    }
}
