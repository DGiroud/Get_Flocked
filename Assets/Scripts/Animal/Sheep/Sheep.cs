using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

public class Sheep : MonoBehaviour {

    GameObject fieldObject;     //Reference to the field, so that we can find a new position relative to it's dimensions
    
    public float radius;
    public int score;
    [Range(0, 1.0f)]
    public float speedModifier;

    public float distToGround;                  //Variable used for IsGrounded();
    [Tooltip("The range of force that the sheep will be moved at when spawned")]
    public Vector2 spawnRangeForce;
    //public float 
    [Tooltip("FOR DEBUG, READ ONLY")]
    private Vector3 newPosDebug;                //Variable so we can actively see where the sheep is trying to go during runtime[DEBUG ONLY]
    private float fieldPosX;
    private float fieldPosZ;
    public string currentBehaviour;

    private Animator sheepAnim;

    // Use this for initialization
    void Start() {
        fieldObject = GameObject.FindWithTag("Field");

        fieldPosX = fieldObject.transform.position.x;
        fieldPosZ = fieldObject.transform.position.z;

        sheepAnim = GetComponent<Animator>();
    }

    void Update()
    {
        //if the sheep's y axis reaches too far into the negatives (meaning that it's fallen off the edge or through teh floor), 
        // put it back into the object pool

        if (transform.position.y < -1)
            SheepManager.Instance.DestroySheep(gameObject);
    }

    #region Set Sheep Behaviours Functions
    public void SetWanderTrue()
    {
        sheepAnim.SetBool("isWandering", true);
        sheepAnim.SetBool("isKicked", false);
        sheepAnim.SetBool("isPushed", false);
    }

    public void SetKickedTrue()
    {
        sheepAnim.SetBool("isKicked", true);
        sheepAnim.SetBool("isWandering", false);
        sheepAnim.SetBool("isPushed", false);
    }

    public void SetPushedTrue()
    {
        sheepAnim.SetBool("isPushed", true);
        sheepAnim.SetBool("isWandering", false);
        sheepAnim.SetBool("isKicked", false);
    }

    public void SetIdleTrue()
    {
        sheepAnim.SetBool("isPushed", false);
        sheepAnim.SetBool("isWandering", false);
        sheepAnim.SetBool("isKicked", false);
    }
    #endregion

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