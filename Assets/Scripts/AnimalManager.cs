using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimalManager : MonoBehaviour
{
    // singleton instance
    private static AnimalManager instance;

    // prefabs
    [Header("Prefabs")]
    public GameObject sheepPrefab; // reference to sheep
    public GameObject ramPrefab; // reference to ram
    public GameObject[] patternedSheepPrefabs; // reference to patterned sheep

    // sheep management
    [Header("Sheep Management")]
    [Range(0, 30)]
    [Tooltip("Maximum number of sheep on-screen at any given time")]
    public int maximumSheep; // max number of sheep at any given time
    private GameObject[] sheepPool; // the collection of sheep
    private Queue<GameObject> availableSheep; // sheep which can be spawned

    // sheep spawning
    [Tooltip("Minimum time (in seconds) between spawns")]
    public float sheepSpawnRate; // minimum time between spawns
    private float sheepSpawnTimer = 0.0f; // timer used to determine cooldown
    public Transform[] sheepSpawnPoints; // spawn points

    // ram spawning
    [Header("Ram Management")]
    [Tooltip("Minimum time (in seconds) between spawns")]
    public float ramSpawnRate;
    private float ramSpawnTimer = 0.0f;
    public Transform ramSpawnPoint;
    
    /// <summary>
    /// getter for singleton instance of AnimalManager
    /// </summary>
    public static AnimalManager Instance
    {
        get
        {
            return instance;
        }
    }

    /// <summary>
    /// initialises the sheep pool, and sheep queue. Sheep are added to the
    /// pool, set to inactive and added to the queue of available sheep
    /// </summary>
    void Awake ()
    {
        // assign singleton instance
        instance = this;

        // initialise pool
        sheepPool = new GameObject[maximumSheep];
        availableSheep = new Queue<GameObject>();

        for (int i = 0; i < maximumSheep; i++)
        {
            // create a sheep
            GameObject sheep = Instantiate<GameObject>(sheepPrefab);

            sheepPool[i] = sheep; // add sheep to pool
            sheepPool[i].SetActive(false); // all sheep inactive on start-up
            availableSheep.Enqueue(sheepPool[i]); // all sheep ready to spawn
        }

        // create the ram
        Instantiate<GameObject>(ramPrefab);
        ramPrefab.SetActive(false); // hide by default
	}
	
	/// <summary>
    /// increments spawn timers. Said timers are used for measuring the 
    /// cooldown of the sheep and ram, and ultimately whether it can spawn
    /// </summary>
	void Update ()
    {
        // attempt to spawn sheep or ram
        SpawnSheep();
        SpawnRam();

        // increment spawn timers
        sheepSpawnTimer += Time.deltaTime;
        ramSpawnTimer += Time.deltaTime;

        DebugKeys(); // delete this later
    }

    /// <summary>
    /// gets an available sheep from the pool if possible, places it 
    /// on the scene and sets to active.
    /// </summary>
    public void SpawnSheep()
    {
        // safety check, don't spawn sheep if there's none available
        if (availableSheep.Count == 0)
            return;

        // don't spawn sheep if the previous spawn is still on cooldown
        if (sheepSpawnTimer < sheepSpawnRate + GetSheepCount())
            return;

        // get next available sheep
        GameObject nextSheep = availableSheep.Dequeue();

        // set sheep position (randomly*) to one of the spawn points
        Transform randomSpawnPoint = sheepSpawnPoints[Random.Range(0, sheepSpawnPoints.Length)];
        nextSheep.transform.position = randomSpawnPoint.position;
        nextSheep.SetActive(true); // show sheep

        // reset spawn timer;
        sheepSpawnTimer = 0.0f;
    }

    /// <summary>
    /// places the ram on the scene and enables it, if possible
    /// </summary>
    public void SpawnRam()
    {
        // safety check, don't spawn ram if there's one already spawned
        if (ramPrefab.activeSelf)
            return;

        // don't spawn ram if the previous spawn is still on cooldown
        if (ramSpawnTimer < ramSpawnRate)
            return;

        // set ram position to spawn point
        ramPrefab.transform.position = ramSpawnPoint.position;
        ramPrefab.SetActive(true); // show ram

        // reset spawn timer
        ramSpawnTimer = 0.0f;
    }

    /// <summary>
    /// sets the specified sheep to inactive, and adds it to 
    /// availability queue
    /// </summary>
    /// <param name="sheep">the desired sheep to delete/destroy</param>
    public void DestroySheep(GameObject sheep)
    {
        availableSheep.Enqueue(sheep); // sheep is now available
        sheep.SetActive(false); // hide sheep
    }

    /// <summary>
    /// sets the single ram in scene to inactive
    /// </summary>
    public void DestroyRam()
    {
        ramPrefab.SetActive(false); // hide ram
    }

    /// <summary>
    /// simple helper function for returning the amount of spawned sheep
    /// </summary>
    /// <returns>current quantity of sheep in the scene</returns>
    public int GetSheepCount()
    {
        // sheep count = total sheep - available (inactive) sheep
        return maximumSheep - availableSheep.Count;
    }

    /// <summary>
    /// simple debug function which spawns sheep when "1" is pressed, and
    /// spawns a ram when "2" is pressed
    /// </summary>
    private void DebugKeys()
    {
        // "1" to spawn sheep
        if (Input.GetKeyDown(KeyCode.Alpha1))
            SpawnSheep(); 

        // "2" to spawn ram
        if (Input.GetKeyDown(KeyCode.Alpha2))
            SpawnRam();
    }
}
