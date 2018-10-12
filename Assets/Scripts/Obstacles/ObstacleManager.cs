using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Obstacle
{
    public GameObject prefab;
    public int amountPerQuadrant;
}

public class ObstacleManager : MonoBehaviour {

    public GameObject[] spawnPos;
    private float destinationX;
    private float destinationZ;
    
    public Obstacle[] obstaclePrefabs;
    // singleton instance
    #region singleton

    private static ObstacleManager instance;

    /// <summary>
    /// getting for singleton instance of ObstacleManager
    /// </summary>
    public static ObstacleManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    // Use this for initialization
    private void Start ()
    {
        instance = this;
        SpawnObjects();
    }

    void SpawnObjects()
    {
        foreach(GameObject spawnPos in spawnPos)
        {

            //int select = Random.Range(0, spawnObj.Count);
            //Instantiate(spawnObj[select], spawnPos.transform.position, spawnPos.transform.rotation);
            //RandomDestination();
            //Debug.Log("Obsatcle spawned" + spawnObj);


        }
    }

    void RandomDestination()
    {
        Vector3 newPos = new Vector3();
        destinationX = newPos.x;
        destinationZ = newPos.z;
        foreach (GameObject spawnPos in spawnPos)
        {
            newPos.x = UnityEngine.Random.Range(Random.Range(10, -20), Random.Range(0, -0));
            newPos.z = UnityEngine.Random.Range(Random.Range(10, -20), Random.Range(0, -0));



          // float obsSpawnX = spawnPos.transform.position.x;
          // float obsSpawnZ = spawnPos.transform.position.z;
          //
          // newPos.x = UnityEngine.Random.Range(obsSpawnX - spawnPos.transform.position.x / 2,
          //                                     obsSpawnX + spawnPos.transform.position.x / 2);
          //
          // newPos.z = UnityEngine.Random.Range(obsSpawnZ - spawnPos.transform.position.z / 2,
          //                                     obsSpawnZ + spawnPos.transform.position.z / 2);
        }
    }

}
