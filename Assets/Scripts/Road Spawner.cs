using UnityEngine;
using System.Collections.Generic;

public class RoadSpawner : MonoBehaviour
{
    public GameObject roadPrefab;
    public Transform playerTransform;
    public List<GameObject> activeRoads = new List<GameObject>();
    public float spawnZ = 0;
    public float roadLength = 30;
    public int numberOfRoads = 10;

    [Header("Obstacle Settings")]
    public GameObject[] hazardPrefabs;  
    public GameObject[] scorablePrefabs; 

    [Header("Spawn Settings")]
    public int obstaclesPerTile = 2;

    [Header("Difficulty Settings")]
    public int maxObstaclesPerTile = 5; 
    public float difficultyMultiplier = 0.1f;

    [Header("Power-up Collections")]
    public GameObject[] powerUpPrefabs;

    void Start()
    {
        for (int i = 0; i < numberOfRoads; i++)
        {
            SpawnRoad();
        }
    }

    void Update()
    {
        if (playerTransform.position.z > spawnZ - (numberOfRoads * roadLength))
        {
            SpawnRoad();

            if (activeRoads.Count > numberOfRoads + 2)
            {
                DeleteRoad();
            }
        }
    }

    private void DeleteRoad()
    {
        if (activeRoads.Count > 0)
        {
            Destroy(activeRoads[0]);
            activeRoads.RemoveAt(0);
        }
    }

    public void SpawnRoad()
    {
        GameObject go = Instantiate(roadPrefab, transform.forward * spawnZ, roadPrefab.transform.rotation);
        activeRoads.Add(go);

        SpawnObstaclesOnTile(spawnZ, go.transform);

        spawnZ += roadLength;
    }

    void SpawnObstaclesOnTile(float zPos, Transform parent)
    {
        int currentDifficultyCount = Mathf.Min(obstaclesPerTile + Mathf.FloorToInt(zPos / 500 * difficultyMultiplier), maxObstaclesPerTile);

        for (int i = 0; i < currentDifficultyCount; i++)
        {
            if (Random.value > 0.3f)
            {
                float[] lanes = { -2.9f, 0f, 2.9f };
                float randomX = lanes[Random.Range(0, lanes.Length)];
                float randomZ = Random.Range(zPos, zPos + roadLength);
                Vector3 spawnPos = new Vector3(randomX, 0.2f, randomZ);

                GameObject selectedPrefab = null;

                if (Random.value < 0.1f)
                {
                    GameObject selectedPowerUp = powerUpPrefabs[Random.Range(0, powerUpPrefabs.Length)];
                    Instantiate(selectedPowerUp, spawnPos, Quaternion.identity, parent);
                }

                if (Random.value > 0.3f)
                {
                    if (hazardPrefabs.Length > 0)
                        selectedPrefab = hazardPrefabs[Random.Range(0, hazardPrefabs.Length)];
                }
                else
                {
                    if (scorablePrefabs.Length > 0)
                        selectedPrefab = scorablePrefabs[Random.Range(0, scorablePrefabs.Length)];
                }

                if (selectedPrefab != null)
                {
                    GameObject obs = Instantiate(selectedPrefab, spawnPos, selectedPrefab.transform.rotation);
                    obs.transform.SetParent(parent);
                }
            }
        }
    }
}