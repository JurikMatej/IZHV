using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class ObstacleSpawner : MonoBehaviour
{
    [Serializable]
    public struct SpawnableObstacle
    {
        // Reference to a prefab obstacle
        public GameObject objRef;

        // Probability distribution range
        [Range(0f, 1f)] 
        public float spawnChance;
    }

    // Obstacles available for spawning
    public SpawnableObstacle[] spawnableObstacles;

    public float spawnRate = 1.5f;
    public float spawnRateBase = 1f;

    private void OnEnable()
    {
        // Start spawning obstacles
        Invoke(nameof(SpawnObstacle), Random.Range(spawnRateBase, spawnRate));
    }

    private void OnDisable()
    {
        CancelInvoke(nameof(SpawnObstacle));
    }

    private void SpawnObstacle()
    {
        float spawnChance = Random.value;

        // Loop through the spawnables & use probability distribution to select which one to spawn
        foreach (var obstacle in spawnableObstacles)
        {
            if (spawnChance <= obstacle.spawnChance)
            {
                GameObject obstacleToSpawn = Instantiate(obstacle.objRef);
                obstacleToSpawn.transform.position += this.transform.position;
                break;
            }
        }
        
        // Continue spawning via callbacks
        Invoke(nameof(SpawnObstacle), Random.Range(spawnRateBase, spawnRate));
    }
}
