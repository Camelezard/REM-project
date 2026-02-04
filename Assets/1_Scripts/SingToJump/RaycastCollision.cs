using UnityEngine;

public class RaycastCollision : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public SingJumping singJumping;

    public float groundRaycastDistance = 0.01f;
    public LayerMask groundLayerMask;

    private RaycastHit2D _GroundHit;
    private GameObject _GroundCollider;
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
        _GroundHit = Physics2D.Raycast(transform.position, Vector2.down, groundRaycastDistance, groundLayerMask);
        Debug.DrawRay(transform.position, Vector2.down * groundRaycastDistance, Color.black, 0.1f);
        

        if (_GroundHit.collider != null)
        { 
            _GroundCollider = _GroundHit.collider.gameObject;
            transform.position = new Vector2(transform.position.x, _GroundHit.point.y);
            singJumping.velocityY = 0;
            singJumping.isFalling = false;
        }
        else
        {
            singJumping.isFalling= true;
        }
    }

    
}
