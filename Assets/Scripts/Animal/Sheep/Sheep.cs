using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;   //Needed for NavMeshAgent
using System;

[System.Serializable]
public struct SheepTier
{
    public Mesh mesh;
    public float trueRadius;
    public float snapRadius;
    public float growthRequirement;
    public int score;
}

public class Sheep : MonoBehaviour {

    GameObject fieldObject;     //Reference to the field, so that we can find a new position relative to it's dimensions
    NavMeshAgent agent;         //Reference to this object
    System.Random rand;         //Random function so that we can generate random positions for the sheep
    private float timer = 0;    //Constantly counting up and resetting, determins when the sheep will search for a new location

    public enum SheepState { Spawn, Idle, Wander, Push, Kick }  //Easy access to the different behaviour states the sheep will have 
                                                                //We do not need an exit state for the sheep, as the sheep will only be removed when it is used to score a goal, and this will
                                                                // be handled by the sheep manager, which will remove it from the scene and place it back into the object pool for later spawning

    //Private getter and setter for the sheep behavioural states
    private SheepState currentState;

    // array of sheep tiers
    [SerializeField]
    private SheepTier[] sheepTiers;
    private SheepTier currentTier;
    public SheepTier CurrentTier { get { return currentTier; } }

    [Header("A.I settings")]
    public float wanderDuration = 5.0f;            //How long the sheep will wander around aimlessly
    public float idleMinRange;
    public float idleMaxRange;
    private float wanderTimer = 0f;                //Timer that will trigger the Wander state's GetNewDestination() function
    [Tooltip("Affects the interval between when a sheep will seek out a new position on the field. 1 = 1 second.")]
    public float rotationSpeed = 10.0f;             //The speed at which the sheep will spin or "Roll"
    [Tooltip("The range that the agent needs to be in of it's desired location before it hits it's idle state")]
    public float destCheckRadius;
    private float randomNumGen;
    
    //Float we use to keep track of when the sheep is grounded, used by the kick functionality
    private float distToGround;                 

    private float growthTimer = 0.0f;            //What will be incremented as the sheep idles, will be checked against growthRequirements
    private int currentLevel = 0;                //Reference to the current level the game is taking place in

    //Private variables so that we can peak into what an agent's destination is during runtime
    private float agentDestinationX;
    private float agentDestinationZ;

    // Use this for initialization
    void Start() {
        fieldObject = GameObject.FindWithTag("Field");

        rand = new System.Random();

        CheckNavMeshAgent();

        //When the sheep is created, we set it's inital destination here
        agent = GetComponent<NavMeshAgent>();
        agent.destination = fieldObject.transform.position;

        distToGround = agent.transform.position.y;

        ResetSheep();
    }

    void Update()
    {
        //Simple timer that checks against the IdleDuration variable, currently used by the sheep spawner
        timer += 1 * Time.deltaTime;

        switch (currentState)
        {
            //--------------------------------------|

            case SheepState.Spawn:
                //When the sheep spawns, we don't want it to have to wait before finding a new location, hence we immediately have it
                // calculate a new random position to seek to

                GetNewDestination();

                ResetSheep();

                if ((transform.position.x <= agent.destination.x + destCheckRadius || transform.position.x >= agent.destination.x - destCheckRadius) ||
                   (transform.position.z <= agent.destination.z + destCheckRadius || transform.position.z >= agent.destination.z - destCheckRadius))
                {
                    SetState(SheepState.Idle);
                }

                //Add animations here (Same as Wander)

                break;

            //--------------------------------------|

            case SheepState.Idle:

                growthTimer += Time.deltaTime;

                //If the sheep has remained idle for an amount of time equal to it's growth requirements, upgrade the sheep to the next tier
                if (currentLevel + 1 < sheepTiers.Length && growthTimer >= currentTier.growthRequirement)
                {
                    LevelUp();
                    growthTimer = 0.0f;
                }

                //If the sheep has remained idle for an amount of time (determined randomly between the specified range), seek a new location
                if (timer > randomNumGen)
                {
                    GetNewDestination();

                    SetState(SheepState.Wander);
                    timer = 0f;
                    randomNumGen = UnityEngine.Random.Range(idleMinRange, idleMaxRange);
                }

                //Add animations here

                break;

            //--------------------------------------|

            case SheepState.Wander:

                wanderTimer += Time.deltaTime;
                
                if (transform.position.x == agent.destination.x && transform.position.z == agent.destination.z)
                { 
                    SetState(SheepState.Idle);
                    break;
                }

                //If the sheep gets stuck in a wander for too long, we force it into Idle
                else if (wanderTimer >= 10.0f)
                {
                    SetState(SheepState.Idle);
                    break;
                }

                //Add animations here (Same as Spawn)

                break;         

            //--------------------------------------|

            case SheepState.Push:
                agent.enabled = false;
                //

                //Simply testing the rotation here
                //GetComponentInChildren<Transform>().transform.Rotate(25.0f, 25f, 25f);

                //Add animations here

                break;

            //--------------------------------------|

            case SheepState.Kick:
                //

                //We need to disable the navMeshAgent when it's being forced off it's desired path, it has weird results otherwise
                if (agent.enabled == true)
                    agent.enabled = false;

                //When the sheep hits the ground, change it's state to idle. We can call this a "stun" for it having been kicked
                if (IsGrounded())
                {
                    float tempTimer = 0.0f;

                    tempTimer += Time.deltaTime;

                    if(tempTimer >= 3.0f)
                    SetState(SheepState.Idle);
                }

                //Add animations here

                break;

            //--------------------------------------|
        }
    }

    private void ResetSheep()
    {
        SetState(SheepState.Spawn);

        currentTier = sheepTiers[0];

        ResetTimers();

        float size = currentTier.trueRadius;
        transform.localScale = new Vector3(size, size, size);
    }

    private void ResetTimers()
    {
        timer = 0.0f;
        growthTimer = 0.0f;
        wanderTimer = 0.0f;
    }

    bool IsGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, distToGround);
    }

    //Checks that this object has a navMeshAgent component, and creates one if it doesn't
    void CheckNavMeshAgent()
    {
        if (this.GetComponent<NavMeshAgent>() == null)
        {
            gameObject.AddComponent(typeof(NavMeshAgent));
        }
    }

    //Simple function to change the sheep's current state;
    public void SetState(SheepState newState)
    {
        currentState = newState;
    }

    public SheepState GetState()
    {
        return currentState;
    }

    //Function to pull a new random position within the confines of the play field
    private void GetNewDestination()
    {
        //Turn the navMeshAgent on if it isn't already
        if (agent.enabled == false)
            agent.enabled = true;

        Vector3 newPos = new Vector3();

        float fieldPosX = fieldObject.transform.position.x;
        float fieldPosZ = fieldObject.transform.position.z;

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

        agentDestinationX = newPos.x;
        agentDestinationZ = newPos.z;

        agent.destination = newPos;

        //before we disable the agent, we need to store the position elsewhere so that we still know where to travel to, otherwise
        // the agent will simply cease

        //This function is not doing as intended. It is correctly changing the rotations inline with the sheep's
        // movements, however it is not changing the rotation of the sheep model
        RotateTowards(newPos); 
    }

    private void RotateTowards(Vector3 destination)
    {
        Vector3 direction = (destination - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

    //Gets called by the idle animation when growth's incrementation matches thr sheep's current growthRequirement,
    //"upgrades" the sheep into the next tier. 
    // Affects Mesh (The sheep model will increase in size), score multiplier, mass, and speed.
    public void LevelUp()
    {
        currentTier = sheepTiers[++currentLevel];

        float size = currentTier.trueRadius;
        transform.localScale = new Vector3(size, size, size);
    }

    public void ResetTier()
    {
    }
}