using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteGenerator : MonoBehaviour
{
    public Transform player;

    public Camera mainCamera;

    public GameObject[] buildingPrefabs;

    public int startBuildings = 6;

    public float spawnAheadDistance = 40f;

    public float destroyingBehindBuildingDistance = 20f;

    public float minGapBetweenBuildings = 2f;

    public float maxGapBetweenBuildings = 6f;

    public float firstGapBetweenBuildings = 0f;

    public float firstBuildingOffsetX = 0f;

    public float baseGroundY = 0f;

    public float minHeightDifference = -2f;

    public float maxHeightDifference = 2f;

    public float minBuildingY = -3f;

    public float maxBuildingY = 8f;

    public Vector3 firstBuildingPosition = Vector3.zero;

    public LayerMask groundLayer;

    private float lastBuildingRightEdge;
    private float lastBuildingY;
    private bool firstPostStartBuildingSpawned;

    private readonly List<GameObject> spawnedBuildings = new List<GameObject>();

    void Start()
    {
        if(mainCamera == null)
        {
            mainCamera = Camera.main;

        }

        SpawnFirstBuilding();

        for(int i = 1; i < startBuildings; i++)
        {
            SpawnBuilding();
        }
    }

    void Update()
    {
        if(player == null || buildingPrefabs == null || buildingPrefabs.Length == 0)
        {
            return;
        }

        float farthestNeed = player.position.x + spawnAheadDistance;

        while(lastBuildingRightEdge < farthestNeed)
        {
            SpawnBuilding();
        }

        DestroyBuildingsBehindPlayer();
    }

    void SpawnFirstBuilding()
    {
        if (buildingPrefabs == null || buildingPrefabs.Length == 0)
        {
            return;
        }

        GameObject prefab = buildingPrefabs[0];
        GameObject building = Instantiate(prefab);

        float width = GetBuildingWidth(building);

        if (width <= 0.01f)
        {
            width = 4f;
        }

        float firstX = firstBuildingPosition.x + firstBuildingOffsetX;
        float firstY = firstBuildingPosition.y;

        building.transform.position = new Vector3(firstX, firstY, firstBuildingPosition.z);

        lastBuildingRightEdge = firstX + (width * 0.5f);
        lastBuildingY = baseGroundY;

        spawnedBuildings.Add(building);
    }

    void SpawnBuilding()
    {
        GameObject prefab = buildingPrefabs[Random.Range(1,buildingPrefabs.Length)];
        GameObject building = Instantiate(prefab);

        float width = GetBuildingWidth(building);

        if(width<= 0.01f)
        {
            width = 4f;
        }

        float gap = firstPostStartBuildingSpawned
            ? Random.Range(minGapBetweenBuildings, maxGapBetweenBuildings)
            : firstGapBetweenBuildings;

        if (!firstPostStartBuildingSpawned)
        {
            gap = Mathf.Min(gap, 0f);
        }

        float newX = lastBuildingRightEdge + gap + (width*0.5f);

        float heightDelta = Random.Range(minHeightDifference, maxHeightDifference);

        float newY = Mathf.Clamp(lastBuildingY+heightDelta, minBuildingY,maxBuildingY);

        building.transform.position = new Vector3(newX, newY, 0f);

        lastBuildingRightEdge = newX + (width * 0.5f);

        lastBuildingY = newY;
        firstPostStartBuildingSpawned = true;
        
        spawnedBuildings.Add(building);
        
    }

    float GetBuildingWidth(GameObject building)
    {
        Collider2D[] colliders = building.GetComponentsInChildren<Collider2D>();

        for (int i = 0; i < colliders.Length; i++)
        {
            Collider2D collider2D = colliders[i];

            if (collider2D != null && ((1 << collider2D.gameObject.layer) & groundLayer.value) != 0)
            {
                return collider2D.bounds.size.x;
            }
        }
        return 0f;
    }

    void DestroyBuildingsBehindPlayer()
    {
        float leftEdge;

        if(mainCamera != null)
        {
            float cameraHalfWidth = mainCamera.orthographicSize * mainCamera.aspect;

            leftEdge = mainCamera.transform.position.x - cameraHalfWidth;
        }
        else
        {
            leftEdge = player.position.x - destroyingBehindBuildingDistance;
        }

        for(int i = spawnedBuildings.Count - 1; i>=0; i--)
        {
            GameObject building = spawnedBuildings[i];

            if(building == null)
            {
                spawnedBuildings.RemoveAt(i);
                continue;
            }

            float width = GetBuildingWidth(building);

            float rightEdge = building.transform.position.x + (width*0.5f);

            if(rightEdge<leftEdge- destroyingBehindBuildingDistance)
            {
                Destroy(building);
                spawnedBuildings.RemoveAt(i);
            }
        }
    } 
}