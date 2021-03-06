﻿using System.Collections;
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
    private GameObject ramPrefab;  //Reference to first Ram
    #endregion

    // ram spawning variables
    #region spawning
    private GameObject ram;
    [Header("Spawning")]
    [SerializeField]
    [Tooltip("Fill this with a transform (empty game object)")]
    private Transform ramSpawnPoint;  //Reference to first Ram's spawnpoint
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
        ram = Instantiate(ramPrefab, new Vector3(0, 100, 0), new Quaternion());
        
        ram.SetActive(false); // hide by default

        InitialiseRam();
    }
	
	void Update ()
    {
    }

    public void InitialiseRam()
    {
        //ram.transform.position.Set(0, 100, 0);
        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
        ram.GetComponentInChildren<SkinnedMeshRenderer>().enabled = false;
        ram.SetActive(true);    }

    // sets the single ram in scene to inactive  
    public void DestroyRam()
    {
        ram.SetActive(false); // hide ram
    }
}
