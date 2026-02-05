using UnityEngine;

public class RaycastCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SingJumping singJumping;

    public float groundRaycastDistance = 0.01f;
    public LayerMask groundLayerMask;

    public bool isDeactivated = false;

    private RaycastHit2D _GroundHit;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        GroundCollision();
    }

    private void GroundCollision()
    {
        if (isDeactivated) return;

        _GroundHit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        Debug.DrawRay(transform.position, Vector2.down * groundRaycastDistance, Color.black, 0.1f);
        

        if (_GroundHit.collider != null)
        { 
            transform.position = new Vector2(transform.position.x, _GroundHit.point.y);
            singJumping.velocityY = 0;
            singJumping.isFalling = false;
            singJumping.isInAir = false;
        }
        else
        {
            singJumping.isFalling= true;
            singJumping.isInAir = true;
        }
    }

    
}
