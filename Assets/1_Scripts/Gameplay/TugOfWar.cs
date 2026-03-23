using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class TugOfWar : MonoBehaviour
{
    [Header("Players")]
    [SerializeField] private Transform _PalyerContainer;
    [SerializeField] private Transform _playerLeft;
    [SerializeField] private Transform _playerRight;
    [SerializeField] private Transform _LeftFlag;
    [SerializeField] private Transform _RightFlag;
    [SerializeField] private Transform _MiddleFlag;

    [Header("Forces")]
    [SerializeField] private float _pullForce = 0.02f;
    [SerializeField] private float _pullMaxForce = .5f;
    [SerializeField] private float _verticalBonus = 1.5f;



    [Header("Movement")]
    [SerializeField] private float minSwipeDistancePercent = 0.05f;
    [SerializeField] private float deceleration = 0.95f;
    [SerializeField] float force;

    [SerializeField] Text _AnuncementText;

    [Header("Ready Set Go")]
    [SerializeField] private float _readyTime = 1f;
    [SerializeField] private float _setTime = 1f;
    [SerializeField] private float _goTime = 1f;

    [Header("Transition")]
    [SerializeField] private float _WinShowTime = 2;

    private Player _Winer;
    private Vector2? startPos = null;
    private Vector3 pullVelocity = Vector3.zero;

    private bool _CannPull = true;


    #region  abonement


    //Abonnements
    private void OnEnable()
    {
        MultiTouchSwipeDetector.OnSwipeUpdate += OnSwipeUpdate;
        MultiTouchSwipeDetector.OnSwipeEnd += OnSwipeEnd;
    }

    private void OnDisable()
    {
        MultiTouchSwipeDetector.OnSwipeUpdate -= OnSwipeUpdate;
        MultiTouchSwipeDetector.OnSwipeEnd -= OnSwipeEnd;
    }
    #endregion

    void Start()
    {
        StartCoroutine(ReadySetGo());
    }

    private void Update()
    {
        Deceleration();
        CheckWin();
    }

    private void OnSwipeUpdate(SwipeData swipe)
    {
        if (_CannPull)
        {
            force = swipe.velocity.x * _pullForce;

            pullVelocity.x += force;
            pullVelocity.x = Mathf.Clamp(pullVelocity.x, -_pullMaxForce, _pullMaxForce);
        }
    }

    // Start
    private IEnumerator ReadySetGo()
    {
        _CannPull = false;

        _AnuncementText.text = "A vos marques";
        yield return new WaitForSeconds(_readyTime);

        _AnuncementText.text = "Pret ?";
        yield return new WaitForSeconds(_setTime);

        _AnuncementText.text = "Tirez !";
        _CannPull = true;

        yield return new WaitForSeconds(_goTime);
    }


    //Win
    private void CheckWin()
    {
        if (_CannPull)
        {
            if (_MiddleFlag.transform.position.x <= _LeftFlag.transform.position.x)
            {
                _Winer = GameManager.GetInstance().GetPlayerOne();
            }
            else if (_MiddleFlag.transform.position.x >= _RightFlag.transform.position.x)
            {
                _Winer = GameManager.GetInstance().GetPlayerTwo();
            }
                //Win();
        }
    }

    private void Win()
    {
        print($"P{_Winer} win"); 

        StartCoroutine(ShowWin());
        _CannPull = false;
    }

    private IEnumerator ShowWin()
    {
        float ElapsTime = 0;
        while (ElapsTime < _WinShowTime)
        {
            ElapsTime += Time.deltaTime;
            yield return null;
        }
        //ResetTogOfWarGame();

        GameManager.GetInstance().WinGame(_Winer);
    }

    private void ResetTogOfWarGame()
    {
        print("reset");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Fisics
    private void Deceleration()
    {
        _PalyerContainer.transform.position += pullVelocity * Time.deltaTime;
        pullVelocity *= deceleration;
    }


    // Maths
    private Vector3 ScreenToWorld(Vector2 screenPos)
    {
        Vector3 screenPositionWithZ = new Vector3(screenPos.x, screenPos.y, 10f);
        return Camera.main.ScreenToWorldPoint(screenPositionWithZ);
    }


    #region visual feedback
    // Debug

    [SerializeField] private LineRenderer swipeLinePrefab = new LineRenderer();

    public void DrawSwipeLine(Vector3 startWorld, Vector3 endWorld)
    {
        if (swipeLinePrefab == null) return;

        LineRenderer line = Instantiate(swipeLinePrefab);
        line.positionCount = 2;
        line.SetPosition(0, startWorld);
        line.SetPosition(1, endWorld);

        StartCoroutine(DestroyLineAfterSeconds(line, 1f));
    }

    private IEnumerator DestroyLineAfterSeconds(LineRenderer line, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        if (line != null) Destroy(line.gameObject);
    }

    private void OnSwipeEnd(SwipeData swipe)
    {
        DrawSwipeLine(
            ScreenToWorld(swipe.startPos),
            ScreenToWorld(swipe.currentPos)
        );
    }

    #endregion
}
