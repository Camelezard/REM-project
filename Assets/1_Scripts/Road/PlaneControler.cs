using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlaneControler : MonoBehaviour
{
    [SerializeField] private float forwardSpeed = 5f;
    [SerializeField] private float boostSpeed = 8.5f;
    [SerializeField] private float horizontalSpeed = 8f;
    [SerializeField] private float tiltAmount = 20f;
    [SerializeField] private float slowSpeed = 2.5f;
    [SerializeField] private int playerId = 1;
    private float slowTime = 0;

    private float boostTime = 0;

    private float minX;
    private float maxX;

    public int playerID;

    private float horizontalInput;


    private void Start()
    {
        transform.position =
        new Vector3(transform.position.x,
        RoadManager.instance.startRoadPoint.transform.position.y,
        transform.position.z);

        minX = RoadManager.instance.leftRoadBound.position.x;
        maxX = RoadManager.instance.rightRoadBound.position.x;
    }

    void Update()
    {
        HandleTouchInput();
        UpdateBoost();
        MovePlane();
        TiltPlane();
        CheckWin();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        RoadPowerUp lPowerUp = other.GetComponent<RoadPowerUp>();

        if (lPowerUp != null)
        {
            if (lPowerUp.isMalus)
                Slow(lPowerUp);
            else
                Boost(lPowerUp);
        }
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
        float currentSpeed = forwardSpeed;

        if (boostTime > 0)
            currentSpeed = boostSpeed;
        else if (slowTime > 0)
            currentSpeed = slowSpeed;

        Vector3 movement = new Vector3(horizontalInput * horizontalSpeed, currentSpeed, 0);
        transform.Translate(movement * Time.deltaTime);
    }

    void TiltPlane()
    {
        float targetZRotation = -horizontalInput * tiltAmount;
        transform.rotation = Quaternion.Lerp(transform.rotation,
                                             Quaternion.Euler(0, 0, targetZRotation),
                                             Time.deltaTime * 5f);

        transform.position = new Vector3(
        Mathf.Clamp(transform.position.x, minX, maxX),
        transform.position.y,
        transform.position.z
        );
    }

    private void Boost(RoadPowerUp powerUp)
    {
        if (powerUp.boostTime > boostTime)
        {
            boostTime = powerUp.boostTime;
        }
    }

    private void Slow(RoadPowerUp powerUp)
    {
        if (powerUp.boostTime > slowTime)
        {
            slowTime = powerUp.boostTime;
        }
    }

    void UpdateBoost()
    {
        if (boostTime > 0)
        {
            boostTime -= Time.deltaTime;
        }

        if (slowTime > 0)
        {
            slowTime -= Time.deltaTime;
        }
    }

    void CheckWin()
    {
        if (transform.position.y >= RoadManager.instance.endRoadPoint.position.y)
        {
            GameManager.GetInstance().WinGame(GameManager.GetInstance().GetPlayerInList(playerID));
        }
    }
}
