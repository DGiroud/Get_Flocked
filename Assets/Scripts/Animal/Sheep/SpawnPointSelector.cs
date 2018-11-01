using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSelector : MonoBehaviour
{
    private GameObject[] spawnPoints;

    // Sequential spawn mode variables
    private int nextSpawnPointID; 

    /// <summary>
    /// copies the spawn points contained in the sheep manager. Also
    /// initialises variables relevant to the different spawn modes
    /// </summary>
    void Awake ()
    {
        spawnPoints = SheepManager.Instance.SpawnPoints;

        // initialise spawn mode-relevant variables
        nextSpawnPointID = 0;
    }

    /// <summary>
    /// selects a spawn point and returns it based off the given spawn
    /// mode parameter
    /// </summary>
    /// <param name="spawnMode">example input: SpawnMode.Random</param>
    /// <returns>the transform of the selected spawn point</returns>
    public GameObject Select(SpawnMode spawnMode = SpawnMode.Sequential)
    {
        switch (spawnMode)
        {
            case SpawnMode.Random: // completely random
                return RandomSpawnPoint();

            case SpawnMode.Sequential: // sequential
                return SequentialSpawnPoint();

            default: // default to sequential
                return SequentialSpawnPoint();
        }
    }

    /// <summary>
    /// accesses a random index of the spawn point array and returns
    /// it's corresponding element
    /// </summary>
    /// <returns>a spawn point at random</returns>
    private GameObject RandomSpawnPoint()
    {
        return spawnPoints[Random.Range(0, spawnPoints.Length)];
    }

    /// <summary>
    /// selects a spawn point out of the spawn point array using an 
    /// increment/ID value
    /// </summary>
    /// <returns>the next spawn point that was linked in the unity editor</returns>
    private GameObject SequentialSpawnPoint()
    {
        // select next spawn point in array and increment at the same time
        GameObject nextSpawnPoint = spawnPoints[nextSpawnPointID];
        nextSpawnPointID++;

        // if increment value goes out of index range, reset to zero
        if (nextSpawnPointID >= spawnPoints.Length)
            nextSpawnPointID = 0;

        return nextSpawnPoint;
    }
}
