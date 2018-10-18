using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //Needed for NavMeshAgent
using System;

[System.Serializable]
public struct SheepTier
{
    public Mesh mesh;
    public float radius;
    public int score;
    [Range(0, 1.0f)]
    public float speedModifier;
}

//**********************************************************************************************************************
// WE ARE DESTROYING THE NAVMESH AND ALL INSTANCES OF THE NAVMESHAGENT
//**********************************************************************************************************************

public class Sheep : MonoBehaviour {

    GameObject fieldObject;     //Reference to the field, so that we can find a new position relative to it's dimensions

    // array of sheep tiers
    [SerializeField]
    private SheepTier[] sheepTiers;
    private SheepTier currentTier;
    public SheepTier CurrentTier { get { return currentTier; } }
    
    [Tooltip("The range that the agent needs to be in of it's desired location before it hits it's idle state")]
    public float destCheckRadius;
    public float distToGround;                  //Variable used for IsGrounded();
    [Tooltip("The range of force that the sheep will be moved at when spawned")]
    public Vector2 spawnRangeForce;
    //public float 
    private Vector3 newPosDebug;                //Variable so we can actively see where the sheep is trying to go during runtime
    private float fieldPosX;
    private float fieldPosZ;    


    // Use this for initialization
    void Start() {
        fieldObject = GameObject.FindWithTag("Field");


        fieldPosX = fieldObject.transform.position.x;
        fieldPosZ = fieldObject.transform.position.z;
    }

    void Update()
    {
        
    }

    public void ResetSheep()
    {
        currentTier = sheepTiers[0];

        float size = currentTier.radius;
        transform.localScale = new Vector3(size, size, size);
    }

     public bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround);
    }

    //Function to pull a new random position within the confines of the play field
    public Vector3 GetNewDestination()
    {
        Vector3 newPos = new Vector3();        

        #region Comments
        //Here we are getting the current field's position (as it may change in size through playtesting) and setting a new random point
        // within the field limits by getting the localScale (size) and halving it, which will give us the correct dimensions in all directions.
        // E.G. If the width of the field is 25, we want 12.5 from the center going in both directions, so we would find a random point in a range
        // between -12.5 and 12.5;
        #endregion
        newPos.x = UnityEngine.Random.Range(fieldPosX - fieldObject.transform.localScale.x / 2.5f,
                                            fieldPosX + fieldObject.transform.localScale.x / 2.5f);

        newPos.z = UnityEngine.Random.Range(fieldPosZ - fieldObject.transform.localScale.z / 2.5f,
                                            fieldPosZ + fieldObject.transform.localScale.z / 2.5f);
        newPosDebug = newPos;

        return newPos;
        //We've changed this function to now return a new position into the object calling it, rather than directly setting the direction
        // for the agent
    }

    //Function checks if a sheep has moved outside of the given play field area
    public bool CheckOutsideField(GameObject sheep)
    {
        if (sheep.transform.position.x >= fieldPosX + fieldObject.transform.localScale.x / 2.5f ||
            sheep.transform.position.x <= fieldPosX - fieldObject.transform.localScale.x / 2.5f ||
            sheep.transform.position.z >= fieldPosZ + fieldObject.transform.localScale.z / 2.5f ||
            sheep.transform.position.z <= fieldPosZ - fieldObject.transform.localScale.z / 2.5f)
        {
            return true;
        }

        else return false;
    }
}