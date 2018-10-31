using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Ram : MonoBehaviour {

    NavMeshAgent agent;

    //************************
    //******* RAMPAGE ********
    //************************

    public enum RamState { Spawn, Idle, Stepback, Charge}
    private RamState currentState;
    private bool isCharging = false;

    private float acceleration;
    private float stepBackTimer = 0.0f;
    private float idleTimer = 0.0f;
    public float stepBackDuration;
    [Tooltip("The duration that the Ram will wait to spawn after locking in it's location to land")]
    public float spawnTimer;
    [Tooltip("The duration that the Ram will be stunned for after charging and landing")]
    [Range(0, 10)]
    public float stunDuration;
    [Tooltip("The speed at which the Ram will wander around the playing field")]
    [Range(0, 100)]
    public float wanderSpeed;

    GameObject fieldObject;     //Reference to the field, so that we can find a new position relative to it's dimensions
    private float fieldPosX;
    private float fieldPosZ;

    public GameObject crashEffect;

    void Start () {
        SetState(RamState.Spawn);

        fieldObject = GameObject.FindWithTag("Field");

        fieldPosX = fieldObject.transform.position.x;
        fieldPosZ = fieldObject.transform.position.z;
    }

    // Update is called once per frame
    void Update () {		
    }

    void SetState(RamState newState)
    {
        currentState = newState;
    }

    RamState GetState()
    {
        return currentState;
    }

    public Vector3 GetNewDestination()
    {
        Vector3 newPos = new Vector3();

        #region Comments
        //Here we are getting the current field's position (as it may change in size through playtesting) and setting a new random point
        // within the field limits by getting the localScale (size) and halving it, which will give us the correct dimensions in all directions.
        // E.G. If the width of the field is 25, we want 12.5 from the center going in both directions, so we would find a random point in a
        // range between -12.5 and 12.5;
        #endregion
        newPos.x = UnityEngine.Random.Range(fieldPosX - fieldObject.transform.localScale.x / 2f,
            fieldPosX + fieldObject.transform.localScale.x / 2f);

        newPos.z = UnityEngine.Random.Range(fieldPosZ - fieldObject.transform.localScale.z / 2f,
            fieldPosZ + fieldObject.transform.localScale.z / 2f);

        return newPos;
    }
}
