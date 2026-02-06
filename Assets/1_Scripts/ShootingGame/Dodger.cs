using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.InputSystem;

public class Dodger : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected float m_speed = 5f;
    protected Vector2 m_velocity;

    protected float m_screenWidth;

    protected InputAction m_movementAction;
    [SerializeField] protected string m_movementActionPath;
    protected virtual void Start()
    {
        m_movementAction = InputSystem.actions.FindAction(m_movementActionPath);
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        Movement();
    }

    protected virtual void Movement()
    {
        //// Vérifie s'il y a des touches actives
        //if (Touchscreen.current != null && Touchscreen.current.primaryTouch.press.isPressed)
        //{
        //    // Récupère la position du toucher
        //    Vector2 touchPosition = Touchscreen.current.primaryTouch.position.ReadValue();

        //    // Détermine si c'est le côté gauche ou droit de l'écran
        //    float screenWidth = Screen.width;


        //}
        m_velocity.x = m_movementAction.ReadValue<float>();
        Debug.Log(m_velocity);
        transform.position += new Vector3(m_velocity.x, 0, 0) * m_speed * Time.deltaTime;

        
    }
}
