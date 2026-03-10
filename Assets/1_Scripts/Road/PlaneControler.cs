using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneControler : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float horizontalSpeed = 8f;
    [SerializeField] private float tiltAmount = 20f;

    private float minX;
    private float maxX;

    public int playerID;

    private float horizontalInput;

    private void Awake()
    {
        transform.position = new Vector3
            (transform.position.x,
            RoadManager.instance.startRoadPoint.transform.position.y,
            transform.position.z);
    }

    private void Start() {
        minX = RoadManager.instance.rightRoadBound.transform.position.x;
        maxX = RoadManager.instance.leftRoadBound.transform.position.x;
    }

    void Update()
    {
        HandleTouchInput();
        MovePlane();
        TiltPlane();
    }

    void HandleTouchInput()
    {
        horizontalInput = 0;

        if (Touchscreen.current == null)
            return;

        foreach (var touch in Touchscreen.current.touches)
        {
            if (!touch.press.isPressed)
                continue;

            float touchX = touch.position.ReadValue().x;

            if (playerID == 1 && touchX < Screen.width / 2)
                horizontalInput = GetDirection(touchX);

            else if (playerID == 2 && touchX > Screen.width / 2)
                horizontalInput = GetDirection(touchX);
        }
    }

    float GetDirection(float touchX)
    {
        float screenCenter = (playerID == 1) ? Screen.width / 4 : Screen.width * 0.75f;
        return Mathf.Clamp((touchX - screenCenter) / (Screen.width / 4), -1f, 1f);
    }

    float GetMouseDirection(float mouseX)
    {
        float screenCenter = (playerID == 1) ? Screen.width / 4 : Screen.width * 0.75f;
        return Mathf.Clamp((mouseX - screenCenter) / (Screen.width / 4), -1f, 1f);
    }

    float GetDirection(Touch touch)
    {
        float screenCenter = Screen.width / 2;

        if (playerID == 1)
            screenCenter = Screen.width / 4;
        else
            screenCenter = Screen.width * 0.75f;

        return Mathf.Clamp((touch.position.x - screenCenter) / (Screen.width / 4), -1f, 1f);
    }

    void MovePlane()
    {
        Vector3 movement = new Vector3(horizontalInput * horizontalSpeed, forwardSpeed, 0);
        transform.Translate(movement * Time.deltaTime);
    }

    void TiltPlane()
    {
        float targetZRotation = -horizontalInput * tiltAmount;
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.Euler(0, 0, targetZRotation),
                                             Time.deltaTime * 5f);

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.y, minX, maxX),
        transform.position.y,
        transform.position.z
        );
    }
}
