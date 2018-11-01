﻿using System.Collections;
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
    
    public float stepBackDuration;
    [Tooltip("The duration that the Ram will wait to spawn after locking in it's location to land")]
    public float spawnTimer;
    [Tooltip("The duration that the Ram will be stunned for after charging and landing")]
    [Range(0, 10)]
    public float stunDuration;
    [Tooltip("The speed at which the Ram will wander around the playing field")]
    [Range(0, 100)]
    public float wanderSpeed;[Tooltip("How long the Ram waits (in seconds) before finding a new location after reaching it's original")]
    public float idleTime;

    public Vector3 newPos;
    private GameObject[] fieldBox;
    private int previousNum;
    private int rand;

    public GameObject crashEffect;

    void Start () {
        SetState(RamState.Spawn);

        //Creating the FieldBox array;
        fieldBox = new GameObject[4];

        //Initialising each element of our field array to the 4 field objects in our scene
        fieldBox[0] = GameObject.Find("FieldTop");
        fieldBox[1] = GameObject.Find("FieldRight");
        fieldBox[2] = GameObject.Find("FieldBottom");
        fieldBox[3] = GameObject.Find("FieldLeft");
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

    //pls dont break mr ram
    public Vector3 GetNewDestination()
    {
        Vector3 newPos = new Vector3();
        
        //Randomly select one of the 4 fieldBoxes in the scene
        rand = Random.Range(0, 4);

        //We don't want to seek into the same box, but if it happens to get the same box twice, it's earned that.
        if (rand == previousNum)
            rand = Random.Range(0, 4);

        //So that we can check that we don't have the same number as last time
        previousNum = rand;

        //Randomly get an x and z position within that field for our Object to seek to
        newPos.x = Random.Range(fieldBox[rand].transform.position.x - fieldBox[rand].transform.localScale.x / 2f,
                                fieldBox[rand].transform.position.x + fieldBox[rand].transform.localScale.x / 2f);

        newPos.z = Random.Range(fieldBox[rand].transform.position.z - fieldBox[rand].transform.localScale.z / 2f,
                                fieldBox[rand].transform.position.z + fieldBox[rand].transform.localScale.z / 2f);

        return newPos;
    }
}
