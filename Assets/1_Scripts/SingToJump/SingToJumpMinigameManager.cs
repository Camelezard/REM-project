using UnityEngine;

public class SingToJumpMinigameManager : MonoBehaviour
{
    [SerializeField] private GameObject _PlatformObject;
    [SerializeField] private GameObject _FinishLineObject;
    [SerializeField] private Transform _LevelContainer;

    [SerializeField] private int _PlatformNumber = 10;
    [SerializeField] private float _PlatformDistance;
    [SerializeField] private float _DistanceOffset = 9;
    [SerializeField] private float _MinPlatformHeight = -0.6f;
    [SerializeField] private float _MaxPlatformHeight = 1.5f;

    [SerializeField] private float _ScrollingSpeed = 1f;
    void Start()
    {
        InitGame();
    }

    // Update is called once per frame
    void Update()
    {
        _LevelContainer.position += Vector3.left * _ScrollingSpeed * Time.deltaTime;
    }

    private void InitGame()
    {
        GenerateLevel();
    }

    private void GenerateLevel()
    {
        float lCurrentDistance = _DistanceOffset;
        float lCurrentHeight = 0;
        for (int i = 0; i < _PlatformNumber - 1; i++)
        {
            lCurrentHeight = Random.Range(_MinPlatformHeight, _MaxPlatformHeight);

            SpawnPlatform(new Vector3(lCurrentDistance, lCurrentHeight, 0));

            lCurrentDistance += _PlatformDistance;
        }

        lCurrentHeight = _MinPlatformHeight;
        SpawnPlatform(new Vector3(lCurrentDistance, lCurrentHeight, 0));
        Instantiate(_FinishLineObject, new Vector3(lCurrentDistance, 0, 0), Quaternion.identity, _LevelContainer);

    }

    private void SpawnPlatform(Vector3 pPos)
    {
        Instantiate(_PlatformObject, pPos, Quaternion.identity, _LevelContainer);
    }
}
