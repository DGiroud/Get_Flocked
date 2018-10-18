using System;
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
    PseudoRandom, // uses all spawn points but in a random order
    Sequential // uses all points in the order they are linked in the editor
}

[System.Serializable]
public struct PatternedSheep
{
    public GameObject prefab;
    public int amount;
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
    private SheepPool sheepPool;
    private SpawnPointSelector spawnPointSelector;
    #endregion

    // prefabs variables
    #region prefabs
    [Header("Normal Sheep")]

    [SerializeField]
    private GameObject prefab; // reference to sheep
    [SerializeField]
    [Tooltip("Maximum number of sheep on-screen at any given time")]
    private int amount = 10; // maximum amount of sheep

    [Header("Patterned Sheep")]
    [SerializeField]
    private PatternedSheep[] patternedSheep;

    internal void DestroySheep(Sheep sheep)
    {
        throw new NotImplementedException();
    }
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
    public SpawnMode spawnMode = SpawnMode.PseudoRandom; // dictates the order in which sheep spawn

    // spawn points
    [SerializeField]
    [Tooltip("Fill this with transforms (empty game objects)")]
    private Transform[] spawnPoints; // array of transforms for sheep spawns

    // spawn parameters
    [Tooltip("Minimum time (in seconds) between spawns")]
    public float spawnRate = 5.0f; // minimum time between spawns
    [Tooltip("Randomises the spawn rate a little." +
        "\nE.g. if spawn rate is 10 & variance is 2, each sheep may take anywhere between 8-12 seconds to spawn")]
    public float spawnRateVariance = 2.0f; // actual spawn rate = spawn rate +/- rand(variance)
    private float variance = 0.0f;
    private float sheepSpawnTimer = 0.0f; // timer used to determine cooldown
    #endregion

    // variable getters
    #region getters

    public GameObject SheepPrefab { get { return prefab; } }
    public PatternedSheep[] PatternedSheeps { get { return patternedSheep; } }
    public Transform[] SheepSpawnPoints { get { return spawnPoints; } }
    public int MaximumSheep { get { return amount; } }

    #endregion


    /// <summary>
    /// initialises singleton sheep manager instance. Assigns reference
    /// to a sheep pool, and a reference to a spawn point selector
    /// </summary>
    void Awake()
    {
        // assign singleton instance
        instance = this;

        // initialise sheep pool
        sheepPool = gameObject.AddComponent<SheepPool>();

        // initialise spawn point selector
        spawnPointSelector = gameObject.AddComponent<SpawnPointSelector>();

        // initialise patterned sheep selector
        //patternedSheepSelector = gameObject.AddComponent<PatternedSheepSelector>();

        // delete me
        Assert.IsTrue(spawnRateVariance < spawnRate, 
            "Please ensure spawn rate variance is less than spawn rate in the sheep manager");

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
        if (sheepSpawnTimer > spawnRate + variance)
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
        // get sheep from pool
        GameObject nextSheep = sheepPool.TryGetSheep(); 

        // if no available sheep, spawn was a failure
        if (!nextSheep)
            return null;

        // get the next spawn point based off spawnMode
        Transform nextSpawnPoint = spawnPointSelector.Select(spawnMode);

        // spawn it
        nextSheep.transform.position = nextSpawnPoint.position; // adjust position
        nextSheep.SetActive(true); // show sheep

        // adjust spawn variance
        variance = UnityEngine.Random.Range(-spawnRateVariance, spawnRateVariance);

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

        sheepPool.ReturnSheepToPool(sheep); // sheep is now available
        sheep.SetActive(false); // hide sheep
    }
}
