using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //Needed for NavMeshAgent
using System.Timers;    //Needed for Timer
using System;

public class NavMeshNavigation : MonoBehaviour {

    GameObject fieldObject;     //Reference to the field, so that we can find a new position relative to it's dimensions
    NavMeshAgent agent;         //Reference to this object
    System.Random rand;         //Random function so that we can generate random positions for the sheep
    private float timer = 0;    //Constantly counting up and resetting, determins when the sheep will search for a new location

    public enum SheepState { Spawn, Idle, Wander, Push, Kick }  //Easy access to the different behaviour states the sheep will have   

    //Private getter and setter for the sheep behavioural states
    private SheepState CurrentState { get; set; }

    [Header("Sheep Properties")]
    [Tooltip("The 3 seperate sheep models that will be interchanged as the sheep grows")]
    public Mesh[] sheepTiers;               //Container of the different sheep models, being the 3 different sizes
    [Tooltip("An array storing how long each sized ")]
    public float[] growthRequirements;      //Each Tier of sheep will have different requirements for growing in size, represented by a float
    public float[] scoreMultipliers;        //The different sizes of sheep will each have a score multiplier, affecting the score when placed into a goal
    public float wanderDuration;            //How long the sheep will wander around aimlessly

    private float growth;                   //What will be incremented as the sheep idles, will be checked against growthRequirements
    private int currentLevel;               //Reference to the current level the game is taking place in



    [Tooltip("Affects the interval between when a sheep will seek out a new position on the field. 1 = 1 second.")]
    public int idleDuration = 10;

    // Use this for initialization
    void Start() {
        fieldObject = GameObject.FindWithTag("Field");

        rand = new System.Random();

        //When the sheep is created, we set it's inital destination here
        agent = GetComponent<NavMeshAgent>();
        agent.destination = fieldObject.transform.position;

    }

    void Update()
    {
        //Simple timer that checks against the IdleDuration variable, currently used by the sheep spawner
        timer += 1 * Time.deltaTime;

        if (timer >= idleDuration)
        {
            GetNewDestination();
            timer = 0;
        }
    }

    //Function to pull a new random position within the confines of the play field
    private void GetNewDestination()
    {
        Vector3 newPos = new Vector3();

        float fieldPosX = fieldObject.transform.position.x;
        float fieldPosZ = fieldObject.transform.position.z;


        //Here we are getting the current field's position (as it may change in size through playtesting) and setting a new random point
        // within the field limits by getting the localScale (size) and halving it, which will give us the correct dimensions in all directions.
        // E.G. If the width of the field is 25, we want 12.5 from the center going in both directions, so we would find a random point in a range
        // between -12.5 and 12.5;
        newPos.x = UnityEngine.Random.Range(fieldPosX - fieldObject.transform.localScale.x / 2.5f,
                                            fieldPosX + fieldObject.transform.localScale.x / 2.5f);

        newPos.z = UnityEngine.Random.Range(fieldPosZ - fieldObject.transform.localScale.z / 2.5f,
                                            fieldPosZ + fieldObject.transform.localScale.z / 2.5f);

        agent.destination = newPos;

        Debug.Log(agent.destination);
    }

    //Gets called by the idle animation when growth's incrementation matches thr sheep's current growthRequirement,
    //"upgrades" the sheep into the next tier. 
    // Affects Mesh (The sheep model will increase in size), score multiplier, mass, and speed.
    public void LevelUp()
    {

    }
}


//GET CURRENT POSITION, then se that fields current position as a base, and plus/minus 12.5f from that to get a position within the field