using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSpawn : StateMachineBehaviour {

    Light spotlight;
    float timer = 0.0f;
    float spawnTimer = 0.0f;
    private float spawnTimerEnd;
    public float landingSpeed = 0.15f;

    GameObject ram;
    GameObject ramSpawnPoint;
    Vector3 ramTarget;
    bool destination = false;
    bool spawned     = false;
    bool finished    = false;
    bool gameStart   = false;

    //********************************************************************************************************************
    //                                                      NOTES
    //********************************************************************************************************************
    // * Spotlight needs to begin in a random position on the sphere
    // * Spotlight needs to turn off when the Ram hits the target
    // * Spotlight needs to wait for an ingame trigger before spawning. It shouldn't be in the game from the get-go
    //
    // * Ram needs functionality for when it reaches it's target
    // * Ram currently has a bug where it doesn't stick to it's descent path, and instead launches in random directions
    //********************************************************************************************************************

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialise the ram object for us to reference throughout this script
        ram = animator.gameObject;
        //Initialise the spawn point for us the reference throughout this script
        ramSpawnPoint = GameObject.Find("RamSpawnPoint");

        //Finding the light object within the RamManager hierarchy 
        spotlight = RamManager.Instance.GetComponentInChildren<Light>();
        spotlight.enabled = false;

        //Pulling our customisable variable from the Ram prefab
        spawnTimerEnd = animator.GetComponent<Ram>().spawnTimer;        
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //When the round begins, so too should the spotlight. This is a temporary add until we've worked out when exactly to start the Ram
        // event
        if (LevelManager.Instance.gameStart == true)
        {
            spotlight.enabled = true;
            updateSpotlight();
        }

        //If we've hit the ground, create a dust storm, change rotation to be standing upright, and switch state to stunned
        // The cloud of dust should be thick enough that it blocks sight of the Ram briefly, allowing it to simply be snapped
        // back into it's upright position
        if (animator.transform.position.y <= 0.5f)
        {
            //Explosion of dust here
            //Rotation set here
            //Set State here
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //Makes the spotlight flash between red and white
    void FlashRed()
    {
        if (spotlight.color == Color.red)
            spotlight.color = Color.white;

        else if (spotlight.color == Color.white)
            spotlight.color = Color.red;
    }

    public void SpawnRam()
    {
        // set ram position to spawn point
        ram.transform.position = ramSpawnPoint.transform.position;
        //Turn off gravity so the ram flies in a straight line
        ram.GetComponent<Rigidbody>().useGravity = false;
        //Keep the ram looking at it's target as it descends
        ram.transform.LookAt(ramTarget);
        //Freezing the rotation so that when it spawns, it keeps it's visual trajectory
        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;       
    }

    public void updateSpotlight()
    {
        //Shrink the size of the ray throughout it's lifetime
        spotlight.spotAngle -= 0.045f;

        //When the spotlight reaches a small enough size, start flashing red to suggest it's imminent danger
        if (spotlight.spotAngle <= 10 && !finished)
        {
            timer += Time.deltaTime;

            if (timer >= 0.5f)
            {
                FlashRed();
                timer = 0.0f;
            }
        }

        if (spotlight.spotAngle == 1)
        {
            finished = true;
            spawnTimer += Time.deltaTime;
            spotlight.color = Color.red;

            //When the spotlight reaches it's smallest size, lock it in position. This will be where the ram lands.
            //The spawn point will keep moving for a couple seconds however, so that when the Ram spawns and moves towards it's
            // landing point, it will do so at an angle to replicate a meteorite
            if (destination == false)
            {
                ramTarget = spotlight.transform.position;
                ramTarget.y = spotlight.transform.position.y - 20;
                destination = true;
            }

            if (spawnTimer >= spawnTimerEnd && !spawned)
            {
                //We want to spawn the ram at the start, so that the spotlight begins it's function, however we don't want the Ram to be seen yet
                SpawnRam();
                spawned = true;
            }

            if (spawned)
            {
                Vector3 newDir = new Vector3();

                newDir = ramTarget - ram.transform.position;
                newDir = newDir.normalized;

                ram.transform.position += (newDir * landingSpeed);
            }
            spotlight.transform.LookAt(ramTarget);
        }
    }

    public bool IsRamSpawned()
    {
        if (ram.activeSelf == true)
            return true;

        else return false;
    }

    public GameObject GetRam()
    {
        return ram;
    }
}
