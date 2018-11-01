using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct SheepSpawnInfo
{
    public GameObject prefab;
    public Vector2 spawnChance;
}

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private SheepSpawnInfo[] sheep;

    public GameObject SelectSheep()
    {
        // initialise output sheep
        GameObject output = null;
        float random = Random.Range(0, 100); // grab a random number between 0 and 100

        // iterate through the info contained on the spawn point
        for (int i = 0; i < sheep.Length; i++)
        {
            SheepSpawnInfo spawnInfo = sheep[i];

            // use our random number to determine which sheep prefab to spawn
            if (random >= spawnInfo.spawnChance.x &&
                random <= spawnInfo.spawnChance.y)
                output = spawnInfo.prefab;
        }

        // return selected sheep
        return output;
    }
}
