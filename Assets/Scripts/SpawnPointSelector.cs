using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPointSelector : MonoBehaviour
{
    private Transform[] sheepSpawnPoints;

    // Pseudo-Random spawn mode variables
    private Queue<Transform> availableSpawnPoints; 

    // Sequential spawn mode variables
    private int nextSpawnPointID; 


    /// <summary>
    /// copies the spawn points contained in the sheep manager. Also
    /// initialises variables relevant to the different spawn modes
    /// </summary>
    void Awake ()
    {
        // get animal manager's spawn point references
        Transform[] spawnPointsReference = SheepManager.Instance.SheepSpawnPoints;

        // copy spawn point references to a locally defined array
        sheepSpawnPoints = new Transform[spawnPointsReference.Length];
        spawnPointsReference.CopyTo(sheepSpawnPoints, 0);

        // initialise spawn mode-relevant variables
        availableSpawnPoints = new Queue<Transform>();
        nextSpawnPointID = 0;
    }

    /// <summary>
    /// selects a spawn point and returns it based off the given spawn
    /// mode parameter
    /// </summary>
    /// <param name="spawnMode">example input: SpawnMode.Random</param>
    /// <returns>the transform of the selected spawn point</returns>
    public Transform Select(SpawnMode spawnMode)
    {
        switch (spawnMode)
        {
            case SpawnMode.Random: // completely random
                return RandomSpawnPoint();

            case SpawnMode.PseudoRandom: // pseudo-random
                return PseudoRandomSpawnPoint();

            case SpawnMode.Sequential: // sequential
                return SequentialSpawnPoint();

            default: // default to pseudo-random
                return PseudoRandomSpawnPoint();
        }
    }

    /// <summary>
    /// accesses a random index of the spawn point array and returns
    /// it's corresponding element
    /// </summary>
    /// <returns>a spawn point at random</returns>
    private Transform RandomSpawnPoint()
    {
        return sheepSpawnPoints[Random.Range(0, sheepSpawnPoints.Length)];
    }

    /// <summary>
    /// utilises a shuffled queue of available spawn points in order
    /// to use all spawn points but in a random order
    /// </summary>
    /// <returns>the next spawn point from a shuffle queue of spawn points</returns>
    private Transform PseudoRandomSpawnPoint()
    {
        // rebuild spawn point queue when empty
        if (availableSpawnPoints.Count == 0)
            BuildSpawnPointQueue();

        return availableSpawnPoints.Dequeue();
    }

    /// <summary>
    /// selects a spawn point out of the spawn point array using an 
    /// increment/ID value
    /// </summary>
    /// <returns>the next spawn point that was linked in the unity editor</returns>
    private Transform SequentialSpawnPoint()
    {
        // select next spawn point in array and increment at the same time
        Transform nextSpawnPoint = sheepSpawnPoints[nextSpawnPointID++];

        // if increment value goes out of index range, reset to zero
        if (nextSpawnPointID >= sheepSpawnPoints.Length)
            nextSpawnPointID = 0;

        return nextSpawnPoint;
    }

    /// <summary>
    /// 
    /// </summary>
    private void BuildSpawnPointQueue()
    {
        ShuffleSpawnPoints();

        for (int i = 0; i < sheepSpawnPoints.Length; i++)
        {
            availableSpawnPoints.Enqueue(sheepSpawnPoints[i]);
        }

    }

    /// <summary>
    /// 
    /// </summary>
    private void ShuffleSpawnPoints()
    {
        for (int i = 0; i < sheepSpawnPoints.Length; i++)
        {
            Transform temp = sheepSpawnPoints[i];

            int randomIndex = Random.Range(i, sheepSpawnPoints.Length);
            sheepSpawnPoints[i] = sheepSpawnPoints[randomIndex];
            sheepSpawnPoints[randomIndex] = temp;
        }
    }
}
