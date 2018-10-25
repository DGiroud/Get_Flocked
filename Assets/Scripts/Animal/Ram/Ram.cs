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
    public float destCheckRadius;

    [Tooltip("The ram's acceleration is calculated using exponential growth, this is the variable that determines how quickly that growth is")]
    public float exponentialGrowth;

    void Start () {

        agent = GetComponent<NavMeshAgent>();
        acceleration = agent.acceleration;
        agent.enabled = true;   
        agent.angularSpeed = 0;

        SetState(RamState.Spawn);

    }

    // Update is called once per frame
    void Update () {		

        switch(currentState)
        {
            //--------------------------------------|
            case RamState.Spawn:
                break;
            //--------------------------------------|
            case RamState.Idle:
                //This state will take place only after the Ram has charged a player, and will act like a small "stun" before
                // it charges again

                idleTimer += Time.deltaTime;

                if(idleTimer >= 3.0f)
                {
                    SetState(RamState.Stepback);

                    idleTimer = 0.0f;
                }


                break;
            //--------------------------------------|
            //This is the case that happens before the forward charge, where the ram will take a couple of movements backwards as a "wind up" 
            // before charging forwards towards it's target
            case RamState.Stepback:
                Vector3 temp;

                stepBackTimer += Time.deltaTime;

                temp.x = transform.position.x;
                temp.y = transform.position.y;
                temp.z = transform.position.z + 5;

                agent.destination = temp;

                agent.acceleration = -Mathf.Pow(acceleration, exponentialGrowth);

                if (stepBackTimer >= stepBackDuration)
                {
                    SetState(RamState.Charge);
                    stepBackTimer = 0.0f;
                }

                break;
            //--------------------------------------|
            //In this instance, the Ram will randomly select one of the existing players (or possibly the one with the highest score) and
            // charge them. This will entail a small wind up time before a fast charge in a straight line towards it's target
            case RamState.Charge:

                //ISSUE: the player is null for whatever reason, and so we never get past teh if statement

                //pick a random player everytime we charge 
                int pNum = Random.Range(1, PlayerManager.Instance.players.Count);

                if (isCharging == false)
                {
                    agent.destination = PlayerManager.Instance.players[pNum].transform.position;
                    isCharging = true;
                }

                //Here we check if the Ram is within a certain radius of it's destination, rather than finding the exact position, which it
                // may struggle to reach
                if ((transform.position.x <= agent.destination.x + destCheckRadius || transform.position.x >= agent.destination.x - destCheckRadius) ||
                   (transform.position.z <= agent.destination.z + destCheckRadius || transform.position.z >= agent.destination.z - destCheckRadius))
                {
                    SetState(RamState.Idle);
                }

                //Exponential growth in the Ram's acceleration. This will increase every frame
                agent.acceleration = Mathf.Pow(acceleration, exponentialGrowth);
                agent.speed = 20;

                break;
            //--------------------------------------|
            
        }
    }

    void SetState(RamState newState)
    {
        currentState = newState;
    }

    RamState GetState()
    {
        return currentState;
    }
}
