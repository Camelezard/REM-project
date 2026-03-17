using System.Collections.Generic;
using UnityEngine;

public class RoadManager : MonoBehaviour
{
    public static RoadManager instance;

    [Header("Road Bounds")]
    [SerializeField] public Transform rightRoadBound;
    [SerializeField] public Transform leftRoadBound;
    [SerializeField] public Transform startRoadPoint;
    [SerializeField] public Transform endRoadPoint;

    [Header("PowerUps")]
    [SerializeField] private GameObject positivePowerUps;
    [SerializeField] private GameObject negativePowerUps;

    [Header("Spawn Settings")]
    [SerializeField] private int numberOfLines = 10;            
    [SerializeField] private int maxPowerUpsPerLine = 5;        
    [SerializeField] private float minLineSpacing = 1.5f;       
    [SerializeField] private float maxLineSpacing = 3f;         
    [SerializeField, Range(0f, 1f)] private float chanceSpawn = 0.7f; 

    [Header("PowerUp Settings")]
    [SerializeField] private float powerUpWidth = 1f; // largeur du sprite pour éviter superposition

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
        float currentY = startRoadPoint.position.y;

        for (int i = 0; i < numberOfLines; i++)
        {
            SpawnPowerUpLine(currentY);

            currentY += Random.Range(minLineSpacing, maxLineSpacing);

            if (currentY > endRoadPoint.position.y) break;
        }
    }

    void SpawnPowerUpLine(float yPosition)
    {
        List<float> usedXPositions = new List<float>();

        for (int i = 0; i < maxPowerUpsPerLine; i++)
        {
            if (Random.value >= chanceSpawn) continue;

            // Essayer de générer une position qui ne se superpose pas
            int attempts = 0;
            float randomX = 0;
            bool valid = false;

            while (attempts < 10 && !valid)
            {
                randomX = Random.Range(leftRoadBound.position.x + powerUpWidth, rightRoadBound.position.x - powerUpWidth);
                valid = true;

                foreach (float usedX in usedXPositions)
                {
                    if (Mathf.Abs(randomX - usedX) < powerUpWidth)
                    {
                        valid = false; 
                        break;
                    }
                }

                attempts++;
            }

            if (!valid) continue; 

            usedXPositions.Add(randomX);

            bool positive = Random.value > 0.5f;
            GameObject prefab = positive ? positivePowerUps : negativePowerUps;

            Vector3 spawnPos = new Vector3(randomX, yPosition, 0);
            Instantiate(prefab, spawnPos, Quaternion.identity, transform);
        }
    }
}