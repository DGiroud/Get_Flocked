using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

public class RamManager : MonoBehaviour
{
    // singleton instance 
    #region singleton

    private static RamManager instance;

    /// <summary>
    /// getter for singleton instance of RamManager
    /// </summary>
    public static RamManager Instance { get { return instance; } }

    #endregion

    // prefab variable
    #region prefab
    [Header("Prefab")]
    [SerializeField]
    private GameObject ramPrefab; // reference to ram
    #endregion

    // ram spawning variables
    #region spawning
    private GameObject ram;
    [Header("Spawning")]
    [SerializeField]
    [Tooltip("Fill this with a transform (empty game object)")]
    private Transform ramSpawnPoint;
    public Vector2 spawnTime;
    private float variance = 0.0f;
    private float ramSpawnTimer = 0.0f;
    #endregion


    /// <summary>
    /// intialises singleton ram manager instance. Instantiates the ram
    /// and hides it
    /// </summary>
    void Awake ()
    {
        // assign singleton instance
        instance = this;

        // create the ram
        ram = Instantiate(ramPrefab);
        ram.SetActive(false); // hide by default

        // assign variance upon wake up
        variance = Random.Range(spawnTime.x, spawnTime.y);
    }
	
    /// <summary>
    /// increments spawn timer and checks to see if the timer allows for
    /// a new ram to spawn
    /// </summary>
	void Update ()
    {
        // increment spawn timer
        ramSpawnTimer += Time.deltaTime;

        // spawn cooldown check
        if (ramSpawnTimer > variance)
        {
            ramSpawnTimer = 0.0f;
            SpawnRam();
        }
    }

    /// <summary>
    /// places the ram on the scene and enables it, if possible.
    /// </summary>
    public GameObject SpawnRam()
    {
        // safety check, don't spawn ram if there's one already spawned
        if (ram.activeSelf)
            return null;

        // set ram position to spawn point
        ram.transform.position = ramSpawnPoint.position;
        ram.SetActive(true); // show ram

        // adjust spawn variance
        variance = Random.Range(spawnTime.x, spawnTime.y);

        return ram;
    }

    /// <summary>
    /// sets the single ram in scene to inactive
    /// </summary>
    public void DestroyRam()
    {
        ram.SetActive(false); // hide ram
    }
}
