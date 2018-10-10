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
        // get sheep manager reference for easy access
        SheepManager sheepManager = SheepManager.Instance;

        // initialise pool
        sheepPool = new GameObject[GetPoolSize()];
        availableSheep = new Queue<GameObject>();

        // create normal sheep
        for (int i = 0; i < sheepManager.MaximumSheep; i++)
        {
            InstantiateObject(sheepManager.SheepPrefab, i);
        }

        int currentSheepCount = sheepManager.MaximumSheep;

        for (int i = 0; i < sheepManager.PatternedSheeps.Length; i++)
        {
            PatternedSheep patternedSheep = sheepManager.PatternedSheeps[i];

            for (int j = 0; j < patternedSheep.amount; j++)
            {
                InstantiateObject(patternedSheep.prefab, currentSheepCount++);
            }
        }
    }

    private int GetPoolSize()
    {
        SheepManager sheepManager = SheepManager.Instance;
        int poolSize = sheepManager.MaximumSheep;

        for (int i = 0; i < sheepManager.PatternedSheeps.Length; i++)
        {
            poolSize += sheepManager.PatternedSheeps[i].amount;
        }

        return poolSize;
    }

    private GameObject InstantiateObject(GameObject prefab, int i)
    {
        // create an object
        GameObject instantiatedObject = Instantiate(prefab);

        sheepPool[i] = instantiatedObject; // add sheep to pool
        sheepPool[i].SetActive(false); // all sheep inactive on start-up
        availableSheep.Enqueue(sheepPool[i]); // all sheep ready to spawn

        return sheepPool[i];
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
