using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;

    [SerializeField] public Transform rightRoadBound;
    [SerializeField] public Transform leftRoadBound;
    [SerializeField] public Transform startRoadPoint;
    [SerializeField] public Transform endRoadPoint;

    [Header("PowerUps")]
    [SerializeField] GameObject[] positivePowerUps;
    [SerializeField] GameObject[] negativePowerUps;

    [SerializeField] int numberOfPowerUps = 20;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        GenerateRoadObjects();
    }

    void GenerateRoadObjects()
    {
        for (int i = 0; i < numberOfPowerUps; i++)
        {
            CreatePowerUp();
        }
    }

    void CreatePowerUp()
    {
        float randomX = Random.Range(leftRoadBound.position.x, rightRoadBound.position.x);
        float randomY = Random.Range(startRoadPoint.position.y, endRoadPoint.position.y);

        Vector3 spawnPos = new Vector3(randomX, randomY, 0);

        bool positive = Random.value > 0.5f;

        GameObject prefab;

        if (positive)
            prefab = positivePowerUps[Random.Range(0, positivePowerUps.Length)];
        else
            prefab = negativePowerUps[Random.Range(0, negativePowerUps.Length)];

        Instantiate(prefab, spawnPos, Quaternion.identity, transform);
    }
}
