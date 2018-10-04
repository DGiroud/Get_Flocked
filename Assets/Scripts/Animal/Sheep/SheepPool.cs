using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepPool : MonoBehaviour
{
    private GameObject[] sheepPool; // the collection of sheep
    private Queue<GameObject> availableSheep; // sheep which can be spawned


    /// <summary>
    /// initialises the sheep object pool, instantiates and hides all sheep
    /// </summary>
    void Awake()
    {
        // initialise pool
        sheepPool = new GameObject[SheepManager.Instance.MaximumSheep];
        availableSheep = new Queue<GameObject>();

        for (int i = 0; i < SheepManager.Instance.MaximumSheep; i++)
        {
            // create a sheep
            GameObject sheep = Instantiate(SheepManager.Instance.SheepPrefab);

            sheepPool[i] = sheep; // add sheep to pool
            sheepPool[i].SetActive(false); // all sheep inactive on start-up
            availableSheep.Enqueue(sheepPool[i]); // all sheep ready to spawn
        }
    }

    /// <summary>
    /// attempts to grab a sheep from the available object pool
    /// </summary>
    /// <returns>an available sheep if there is one, otherwise null</returns>
    public GameObject TryGetSheep()
    {
        // safety check, don't spawn sheep if there's none available
        if (!IsSheepAvailable())
            return null;

        // dequeue sheep from availability pool and return it
        return (availableSheep.Dequeue());
    }
    
    /// <summary>
    /// places the given sheep back into the available object pool
    /// </summary>
    /// <param name="sheep">the sheep to return</param>
    public void ReturnSheepToPool(GameObject sheep)
    {
        // enqueue sheep to availability pool
        availableSheep.Enqueue(sheep);
    }

    /// <summary>
    /// simple boolean function which returns true if a sheep is available
    /// to be spawned, false otherwise
    /// </summary>
    /// <returns>returns true if a sheep is available, false otherwise</returns>
    public bool IsSheepAvailable()
    {
        return (availableSheep.Count > 0);
    }

    /// <summary>
    /// simple helper function which returns the amount of spawned sheep
    /// </summary>
    /// <returns>current quantity of sheep in the scene</returns>
    public int GetSheepCount()
    {
        // sheep count = total sheep - available (inactive) sheep
        return (sheepPool.Length - availableSheep.Count);
    }
}
