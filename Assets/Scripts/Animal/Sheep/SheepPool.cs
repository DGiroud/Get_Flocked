using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SheepPool : MonoBehaviour
{
    private GameObject[] sheepPool; // the collection of sheep
    private Queue<GameObject> availableSheep; // sheep which can be spawned

    private int poolSize = 0;

    /// <summary>
    /// initialises the sheep object pool, instantiates and hides all sheep
    /// </summary>
    void Awake()
    {
        // get sheep manager reference for easy access
        SheepManager sheepManager = SheepManager.Instance;

        SetPoolSize();

        // initialise pool
        sheepPool = new GameObject[poolSize];
        availableSheep = new Queue<GameObject>();

        int currentSheepCount = 0;

        for (int i = 0; i < sheepManager.sheeps.Length; i++)
        {
            // get patterned sheep
            SheepType sheep = sheepManager.sheeps[i];

            // spawn amount of this particular patterned sheep
            for (int j = 0; j < sheep.amount; j++)
            {
                InstantiateSheep(sheep.prefab, currentSheepCount++);
            }
        }
        
        // randomise the pool order
        RandomisePool();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private void SetPoolSize()
    {
        SheepManager sheepManager = SheepManager.Instance;

        for (int i = 0; i < sheepManager.sheeps.Length; i++)
        {
            poolSize += sheepManager.sheeps[i].amount;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="prefab"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    private void InstantiateSheep(GameObject prefab, int i)
    {
        // create an object
        GameObject instantiatedSheep = Instantiate(prefab);

        sheepPool[i] = instantiatedSheep; // add sheep to pool
        sheepPool[i].SetActive(false); // all sheep inactive on start-up
        availableSheep.Enqueue(sheepPool[i]); // all sheep ready to spawn
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
    /// 
    /// </summary>
    public void RandomisePool()
    {
        GameObject[] availableSheepCopy;
        availableSheepCopy = new GameObject[availableSheep.Count];

        int iter = 0;
        while (availableSheep.Count != 0)
        {
            availableSheepCopy[iter++] = availableSheep.Dequeue();
        }

        Shuffle(availableSheepCopy);

        for (int i = 0; i < availableSheepCopy.Length; i++)
        {
            availableSheep.Enqueue(availableSheepCopy[i]);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="array"></param>
    public void Shuffle(GameObject[] array)
    {
        for (int i = 0; i < array.Length; i++)
        {
            int randomIndex = Random.Range(i, array.Length);

            GameObject temp = array[i];
            array[i] = array[randomIndex];
            array[randomIndex] = temp;
        }
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

    public GameObject[] GetActiveSheep()
    {
        GameObject[] activeSheep = new GameObject[GetSheepCount()];
        int j = 0;

        // iterate over all sheep in pool
        for (int i = 0; i < sheepPool.Length; i++)
        {
            GameObject sheep = sheepPool[i]; // get a sheep

            if (!availableSheep.Contains(sheep))
                activeSheep[j++] = sheep;
        }

        return activeSheep;
    }
}
