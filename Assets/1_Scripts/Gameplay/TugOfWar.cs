using UnityEngine;
using UnityEngine.InputSystem;

public class TugOfWarManagerNewInput : MonoBehaviour
{
    [SerializeField] private Transform _PlayerLeft;
    [SerializeField] private Transform _PlayerRight;

    [SerializeField] private float _PullForce = 0.02f;

    private Vector2 _leftSwipe;
    private Vector2 _rightSwipe;

    public void OnLeftSwipe(InputAction.CallbackContext context)
    {
        _leftSwipe = context.ReadValue<Vector2>();
        if (_leftSwipe.x < -0.5f)
            PullLeft();
        else if (_leftSwipe.x > 0.5f)
            ReverseLeft();
    }

    public void OnRightSwipe(InputAction.CallbackContext context)
    {
        _rightSwipe = context.ReadValue<Vector2>();
        if (_rightSwipe.x > 0.5f)
            PullRight();
        else if (_rightSwipe.x < -0.5f)
            ReverseRight();
    }

    void PullLeft()
    {
        _PlayerLeft.position += Vector3.left * _PullForce;
        _PlayerRight.position += Vector3.left * _PullForce;
    }
    void PullRight() {  }
    void ReverseLeft() {  }
    void ReverseRight()
    {
        
    }
}