using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleManager : MonoBehaviour {
   
    public Terrain playField;                 
    public int numberOfObjects;               //Number of objects to be placed
    private int currentObjects;               //Amount of objects currently placed on playField
    public GameObject objectToPlace;          //Objects to be placed
    private int playFieldWidth;               //Width of the playField
    private int playFieldLength;              //Length of playField
    private int playFieldposX;                //playField of X position
    private int playFieldposZ;                //playField of Z position        

    public GameObject[] obstaclePrefabs;
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
    private void Start ()
    {
        instance = this;

        //playField of X
        playFieldWidth = (int)playField.terrainData.size.x;
        //playField of z
        playFieldWidth = (int)playField.terrainData.size.z;
        //playField position of x
        playFieldposX = (int)playField.terrainData.size.x;
        //playField position of z
        playFieldposZ = (int)playField.terrainData.size.z;
    }
	
	// Update is called once per frame
	void Update ()
    {
		if(currentObjects <= numberOfObjects)
        {
            //generating a random position for both X and Z
            int posX = Random.Range(playFieldposX, playFieldposX + playFieldWidth);
            int posZ = Random.Range(playFieldposZ, playFieldposZ + playFieldLength);
            //gets the hieght of the terrain
            float posY = Terrain.activeTerrain.SampleHeight(new Vector3(posX, 0, posZ));
            //creates a new GameObject for random positions
            GameObject newObject = Instantiate(objectToPlace, new Vector3(posX, posY, posZ), Quaternion.identity);
            currentObjects += 1;
        }
        #region DebugforCompletedGeneration
        //if (currentObjects == numberOfObjects)
        //{
        //    Debug.Log("generate complete");
        //}
        #endregion
    }
}
