using System.Collections.Generic;
using UnityEngine;

public class InfiniteGenerator : MonoBehaviour
{
    [Header("Targets")]
    public Transform player;
    public Camera mainCamera;
    public GameObject[] buildingPrefabs;

    [Header("Custom Initial Spawn Positions")]
    public Vector3 firstBuildingPosition = Vector3.zero;
    [Tooltip("Set the exact X coordinate where the 2nd building should spawn.")]
    public float secondBuildingXPosition = 12f; 

    [Header("Endless Generation Settings")]
    public float spawnAheadDistance = 40f;
    public float destroyingBehindBuildingDistance = 20f;
    public float minGapBetweenBuildings = 2f;
    public float maxGapBetweenBuildings = 6f;

    [Header("Height Variation Settings")]
    public float minHeightDifference = -2f; 
    public float maxHeightDifference = 2f;  
    public float minBuildingY = -3f;         
    public float maxBuildingY = 8f;          

    [Header("Physics Setup")]
    public LayerMask groundLayer;

    private float lastBuildingRightEdge;
    private float lastBuildingY;
    private int lastSpawnedPrefabIndex = -1;
    private readonly List<GameObject> spawnedBuildings = new List<GameObject>();

    void Start()
    {
        if (mainCamera == null)
        {
            mainCamera = Camera.main;
        }
        
        SpawnStarterBuilding();
        SpawnSecondBuilding();
    }

    void Update()
    {
        if (player == null || buildingPrefabs == null || buildingPrefabs.Length == 0) return;
        
        float farthestNeed = player.position.x + spawnAheadDistance;
        while (lastBuildingRightEdge < farthestNeed)
        {
            SpawnNormalBuilding(Random.Range(minGapBetweenBuildings, maxGapBetweenBuildings));
        }

        DestroyBuildingsBehindPlayer();
    }

    void SpawnStarterBuilding()
    {
        if (buildingPrefabs == null || buildingPrefabs.Length == 0) return;

        GameObject building = Instantiate(buildingPrefabs[0]);
        float width = GetBuildingWidth(building);

        building.transform.position = firstBuildingPosition;
        
        lastBuildingRightEdge = firstBuildingPosition.x + (width * 0.5f);
        lastBuildingY = firstBuildingPosition.y;

        spawnedBuildings.Add(building);
    }

    void SpawnSecondBuilding()
{
    if (buildingPrefabs == null || buildingPrefabs.Length < 2) return;

    GameObject prefab = GetRandomBuildingPrefab(1);
    GameObject building = Instantiate(prefab);
    float width = GetBuildingWidth(building);

    
    float heightDelta = Random.Range(minHeightDifference, maxHeightDifference);
    float newY = lastBuildingY + heightDelta;

    
    if (newY > maxBuildingY)
    {
        newY = lastBuildingY - Mathf.Abs(heightDelta);
    }
    
    else if (newY < minBuildingY)
    {
        newY = lastBuildingY + Mathf.Abs(heightDelta);
    }

    building.transform.position = new Vector3(secondBuildingXPosition, newY, 0f);
    
    lastBuildingRightEdge = secondBuildingXPosition + (width * 0.5f);
    lastBuildingY = newY;

    spawnedBuildings.Add(building);
}

void SpawnNormalBuilding(float gap)
{
    GameObject prefab = GetRandomBuildingPrefab(1);
    GameObject building = Instantiate(prefab);
    float width = GetBuildingWidth(building);
    
    float newX = lastBuildingRightEdge + gap + (width * 0.5f);

    
    float heightDelta = Random.Range(minHeightDifference, maxHeightDifference);
    float newY = lastBuildingY + heightDelta;

    
    if (newY > maxBuildingY)
    {
        newY = lastBuildingY - Mathf.Abs(heightDelta); 
    }
    else if (newY < minBuildingY)
    {
        newY = lastBuildingY + Mathf.Abs(heightDelta); 
    }

    building.transform.position = new Vector3(newX, newY, 0f);

    lastBuildingRightEdge = newX + (width * 0.5f);
    lastBuildingY = newY;
    
    spawnedBuildings.Add(building);
}

    GameObject GetRandomBuildingPrefab(int minIndex)
    {
        if (buildingPrefabs == null || buildingPrefabs.Length == 0) return null;
        if (minIndex < 0) minIndex = 0;
        if (minIndex >= buildingPrefabs.Length) minIndex = buildingPrefabs.Length - 1;

        if (buildingPrefabs.Length - minIndex <= 1)
        {
            lastSpawnedPrefabIndex = minIndex;
            return buildingPrefabs[minIndex];
        }

        int prefabIndex = Random.Range(minIndex, buildingPrefabs.Length);
        if (prefabIndex == lastSpawnedPrefabIndex)
        {
            prefabIndex++;
            if (prefabIndex >= buildingPrefabs.Length)
            {
                prefabIndex = minIndex;
            }
        }

        lastSpawnedPrefabIndex = prefabIndex;
        return buildingPrefabs[prefabIndex];
    }

    float GetBuildingWidth(GameObject building)
    {
        Collider2D[] colliders = building.GetComponentsInChildren<Collider2D>();
        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D col = colliders[i];
            if (col != null && ((1 << col.gameObject.layer) & groundLayer.value) != 0)
            {
                return col.bounds.size.x;
            }
        }
        return 4f; 
    }

    void DestroyBuildingsBehindPlayer()
    {
        if (spawnedBuildings.Count == 0) return;

        float leftEdge;
        if (mainCamera != null)
        {
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;
            leftEdge = mainCamera.transform.position.x - cameraHalfWidth;
        }
        else
        {
            leftEdge = player.position.x - destroyingBehindBuildingDistance;
        }
        
        while (spawnedBuildings.Count > 0)
        {
            GameObject building = spawnedBuildings[0];
            if (building == null)
            {
                spawnedBuildings.RemoveAt(0);
                continue;
            }

            float width = GetBuildingWidth(building);
            float rightEdge = building.transform.position.x + (width * 0.5f);

            if (rightEdge < leftEdge - destroyingBehindBuildingDistance)
            {
                Destroy(building);
                spawnedBuildings.RemoveAt(0);
            }
            else
            {
                break; 
            }
        }
    } 
}