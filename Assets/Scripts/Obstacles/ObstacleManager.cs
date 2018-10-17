using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ObstacleManager : MonoBehaviour
{
    //Positions for X and Z
    private float destinationX;
    private float destinationZ;

    [SerializeField]
    public List<GameObject> spawnZone;          //Spawn zone of the Random Spawn Placement
    public List<GameObject> spawnObj;           //The selected spawn object(s)
    public List<GameObject> spawnedObj;         //Object to be spawned


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
    private void Start()
    {
        instance = this;
        SpawnObjects();
    }

    /// SpawnObjects()
    /// Selects random range for spawn object
    /// Instantiate spawnObj to a random destination and transforms the rotation of the spawn position 
    /// </summary>
    void SpawnObjects()
    {
        foreach (GameObject spawnPos in spawnedObj)
        {
            int select = Random.Range(0, spawnObj.Count);
            Instantiate(spawnObj[select], RandomDestination(), spawnPos.transform.rotation);
        }

    }

    /// RandomDestination()
    /// Setting newPos as Vector3 for the distination points of X and Z and setting them as newPos.x and newPos.z
    /// Assigning randomZone to the function RandomZone()
    /// Setting min/max values then finding a random range for min/max
    /// </summary>
    /// <returns>The new position within the randomZone</returns>
    private Vector3 RandomDestination()
    {
        Vector3 newPos = new Vector3();
        destinationX = newPos.x;
        destinationZ = newPos.z;

        GameObject randomZone = RandomZone();

        #region MinMax
        float xMin = -randomZone.transform.localScale.x * 0.5f + randomZone.transform.position.x;
        float xMax = randomZone.transform.localScale.x * 0.5f + randomZone.transform.position.x;
        float zMin = -randomZone.transform.localScale.z * 0.5f + randomZone.transform.position.z;
        float zMax = randomZone.transform.localScale.z * 0.5f + randomZone.transform.position.z;

        #endregion

        newPos.x = Random.Range(xMin, xMax);
        newPos.z = Random.Range(zMin, zMax);
        return newPos;

    }

    /// RandomZone()
    /// <returns>Returns a random spawnZone for RandomZone </returns>
    GameObject RandomZone()
    {
        return spawnZone[Random.Range(0, spawnZone.Count)];
    }
}
