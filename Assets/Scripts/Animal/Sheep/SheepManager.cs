using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

/// <summary>
/// used to determine the order in which the spawn points are chosen
/// when spawning a sheep
/// </summary>
public enum SpawnMode
{
    Random, // completely random
    Sequential // uses all points in the order they are linked in the editor
}

public class SheepManager : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static SheepManager instance;

    /// <summary>
    /// getter for singleton instance of SheepManager
    /// </summary>
    public static SheepManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    // component variables
    #region components
    private SpawnPointSelector spawnPointSelector;
    #endregion

    // start-up variables
    #region startup
    [Header("Start-up")]

    [SerializeField]
    private bool spawnOnStartup = false;
    [SerializeField]
    private int amountToSpawn = 0;
    #endregion

    // spawning/management variables
    #region spawning
    [Header("Spawning")]

    // spawn mode
    [Tooltip("Random: completely random\n" +
        "Pseudo-Random: uses all spawn points equally, in a random order\n" +
        "Sequential: uses all spawn points in the order they are linked in the editor")]
    public SpawnMode spawnMode = SpawnMode.Sequential; // dictates the order in which sheep spawn
    public int maximumSheep = 10;
    private List<GameObject> spawnedSheep;

    [SerializeField]
    private GameObject[] spawnPoints;
    public GameObject[] SpawnPoints {  get { return spawnPoints; } }

    // spawn parameters
    public Vector2 spawnTime;
    private float variance = 0.0f;
    private float sheepSpawnTimer = 0.0f; // timer used to determine cooldown
    #endregion

    /// <summary>
    /// initialises singleton sheep manager instance. Assigns reference
    /// to a sheep pool, and a reference to a spawn point selector
    /// </summary>
    void Awake()
    {
        // assign singleton instance
        instance = this;

        spawnedSheep = new List<GameObject>();

        // initialise spawn point selector
        spawnPointSelector = gameObject.AddComponent<SpawnPointSelector>();

        if (spawnOnStartup)
            for (int i = 0; i < amountToSpawn; i++)
                SpawnSheep(); // spawn a bunch of sheep right off the bat
    }

    /// <summary>
    /// increments spawn timer. Said timer used for measuring the 
    /// cooldown of the sheep, and ultimately whether it can spawn
    /// </summary>
    void Update()
    {
        // increment spawn timer
        sheepSpawnTimer += Time.deltaTime;

        // spawn cooldown check
        if (sheepSpawnTimer > variance && spawnedSheep.Count < maximumSheep)
        {
            SpawnSheep(); // spawn normal sheep
            sheepSpawnTimer = 0.0f;
        }
    }

    /// <summary>
    /// gets an available sheep from the pool if possible, places it 
    /// on the scene and sets to active.
    /// </summary>
    public GameObject SpawnSheep()
    {
        // adjust spawn variance
        variance = Random.Range(spawnTime.x, spawnTime.y);

        // get the next spawn point based off spawnMode
        GameObject nextSpawnPoint = spawnPointSelector.Select(spawnMode);
        GameObject nextSheep = nextSpawnPoint.GetComponent<SpawnPoint>().SelectSheep();
        spawnedSheep.Add(Instantiate(nextSheep));
        nextSheep.transform.position = nextSpawnPoint.transform.position;

        return nextSheep;
    }

    /// <summary>
    /// sets the specified sheep to inactive, and adds it to 
    /// availability queue
    /// </summary>
    /// <param name="sheep">the desired sheep to delete/destroy</param>
    public void DestroySheep(GameObject sheep)
    {
        // later on replace this code with something cleaner like:
        // if (sheep.currentState == State.Push) then ReleaseSheep()
        BaseActor parentScript = sheep.GetComponentInParent<BaseActor>();

        // if the sheep is still parented to an actor...
        if (parentScript)
            parentScript.ReleaseSheep(); //... release it

        spawnedSheep.Remove(sheep);
        Destroy(sheep);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform FindValuableSheep()
    {
        // initialise helper variables
        Transform output = null;
        float highestWorth = Mathf.NegativeInfinity;

        // iterate over all active sheep
        for (int i = 0; i < spawnedSheep.Count; i++)
        {
            GameObject sheep = spawnedSheep[i]; // get sheep
            Sheep sheepScript = sheep.GetComponent<Sheep>();

            // ignore sheep if it's being held already
            if (sheep.transform.parent)
                continue;

            // get distance between this sheep and CPU
            float currentWorth = sheepScript.score;

            // if new minimum distance, update distance
            if (currentWorth > highestWorth)
            {
                highestWorth = currentWorth;
                output = sheep.transform;
            }
        }

        return null;
    }

    /// <summary>
    /// helper function which scans all the spawned sheep, and determines
    /// which one is the closest
    /// </summary>
    /// <returns>the position of the nearest sheep</returns>
    public GameObject FindNearestSheep(GameObject source)
    {
        // initialise helper variables
        GameObject output = null;
        float minDist = Mathf.Infinity;

        // iterate over all active sheep
        for (int i = 0; i < spawnedSheep.Count; i++)
        {
            GameObject sheep = spawnedSheep[i]; // get sheep

            // ignore sheep if it's being held already
            if (sheep.transform.parent)
                continue;

            // get distance between this sheep and CPU
            float dist = Vector3.Distance(source.transform.position, sheep.transform.position);

            // if new minimum distance, update distance
            if (dist < minDist)
            {
                minDist = dist;
                output = sheep;
            }
        }

        return output;
    }
}
