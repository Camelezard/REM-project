using UnityEngine;
using UnityEngine.InputSystem;

public class TugOfWar : MonoBehaviour
{
    [SerializeField] private InputAction _PlayerInput;
    [SerializeField] private Transform _PlayerLeft;
    [SerializeField] private Transform _PlayerRight;

    [SerializeField] private float _PullForce = 0.02f;

    private Vector2 _leftSwipe;
    private Vector2 _rightSwipe;
    private float _SwipeTolerance;

    public void OnLeftSwipe(InputAction.CallbackContext context)
    {
        _leftSwipe = context.ReadValue<Vector2>();
        if (_leftSwipe.x < -_SwipeTolerance)
            PullLeft();
        else if (_leftSwipe.x > _SwipeTolerance)
            ReverseLeft();
    }

    public void OnRightSwipe(InputAction.CallbackContext context)
    {
        _rightSwipe = context.ReadValue<Vector2>();
        if (_rightSwipe.x > _SwipeTolerance)
            PullRight();
        else if (_rightSwipe.x < -_SwipeTolerance)
            ReverseRight();
    }

    void PullLeft()
    {
        _PlayerLeft.position += Vector3.left * _PullForce;
        _PlayerRight.position += Vector3.left * _PullForce;
    }
    void PullRight()
    {
        _PlayerRight.position += Vector3.left * _PullForce;
        _PlayerRight.position += Vector3.left * _PullForce;
    }
    void ReverseLeft()
    {
        
    }
    void ReverseRight()
    {
        
    }
}