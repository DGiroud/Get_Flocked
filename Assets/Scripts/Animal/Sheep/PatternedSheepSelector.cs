using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternedSheepSelector : MonoBehaviour
{
    private GameObject[] patternedSheep;

    // Sequential spawn mode variables
    private int nextPatternedSheepID;

    /// <summary>
    /// 
    /// </summary>
    void Awake ()
    {
        GameObject[] patternedSheepReference = SheepManager.Instance.PatternedSheepPrefabs;
        patternedSheep = new GameObject[patternedSheepReference.Length];

        for (int i = 0; i < patternedSheepReference.Length; i++)
        {
            GameObject patterned = Instantiate(patternedSheepReference[i]);
            patternedSheep[i] = patterned;
            patternedSheep[i].SetActive(false);
        }
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public GameObject Select()
    {
        return SequentialPatternedSheep();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    private GameObject SequentialPatternedSheep()
    {
        // select next patterned sheep in array and increment at the same time
        GameObject nextPatternedSheep = patternedSheep[nextPatternedSheepID++];
        
        // if increment value goes out of index range, reset to zero
        if (nextPatternedSheepID >= patternedSheep.Length)
            nextPatternedSheepID = 0;

        if (nextPatternedSheep.activeSelf == true)
            return null;

        return nextPatternedSheep;
    }
}
