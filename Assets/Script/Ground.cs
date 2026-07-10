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

    public float baseGroundY = 0f;

    public float minHeightDifference = -2f;

    public float maxHeightDifference = 2f;

    public float minBuildingY = -3f;

    public float maxBuildingY = 8f;

    
}