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
    [Tooltip("The duration that the Ram will wait to spawn after locking in it's location to land")]
    public float spawnTimer;

    [Tooltip("The ram's acceleration is calculated using exponential growth, this is the variable that determines how quickly that growth is")]
    public float exponentialGrowth;

    public GameObject crashEffect;

    void Start () {
        SetState(RamState.Spawn);
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
}
