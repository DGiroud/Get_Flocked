using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSpawn : StateMachineBehaviour {

    //This state is the Ram's intitial behaviour state, which can only be accessed when the Ram is created. 
    // This state leads into:
    //  Stunned: When the Ram has successfully landed after it's descent, it will change to it's "stunned" behaviour


    Light spotlight;
    float timer = 0.0f;
    float spawnTimer = 0.0f;
    private float spawnTimerEnd;
    public float landingSpeed = 0.15f;

    private GameObject crashEffect;

    GameObject ram;
    GameObject ramSpawnPoint;
    Vector3 ramTarget;
    bool destination = false;
    bool spawned     = false;
    bool finished    = false;
    bool gameStart   = false;
    bool crashed     = false;

    //********************************************************************************************************************
    //                                                      NOTES
    //********************************************************************************************************************
    // * Spotlight needs to wait for an ingame trigger before spawning. It shouldn't be in the game from the get-go
    //
    // * Ram currently has a bug where it doesn't stick to it's descent path, and instead launches in random directions
    // * Add a sphere to the ram that applies an impulse force to anything it touches during it's descent
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

        crashEffect = ram.GetComponent<Ram>().crashEffect;
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
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }    

    //----------------------------------------------------------------------------------------------------------|
    //----------------------------------------------------------------------------------------------------------|

    public void updateSpotlight()
    {
        Vector3 newDir = new Vector3();


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

        if (spotlight.spotAngle <= 3)
        {
            finished = true;
            spawnTimer += Time.deltaTime;
            spotlight.color = Color.red;
            spotlight.transform.LookAt(ramTarget);

            //When the spotlight reaches it's smallest size, lock it in position. This will be where the ram lands.
            //The spawn point will keep moving for a couple seconds however, so that when the Ram spawns and moves towards it's
            // landing point, it will do so at an angle to replicate a meteorite
            if (destination == false)
            {
                ramTarget = spotlight.transform.position;
                ramTarget.y = spotlight.transform.position.y - 20;
                destination = true;
            }

            if (spotlight.spotAngle == 1 && !spawned)
            {
                //We want to spawn the ram at the start, so that the spotlight begins it's function, however we don't want the Ram to be seen yet
                SpawnRam();
                //So that we don't have to run this segment of code again
                spawned = true;
                //Makes the light dissapear when the Ram spawns  
                spotlight.intensity = 0;
            }

            if (spawned)
            {
                //Fix for the 50/50 bug that would sometimes send the Ram flying into a random direction when spawned. 
                // This resets it's velocity so that it doesn't have any force to it when spawning, it should just
                //  follow it's path towards it's target.
                ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);

                //Keep the ram looking at it's target as it descends
                if(ram.transform.position.y > 0.5f)
                ram.transform.LookAt(ramTarget);

                newDir = (ramTarget - ram.transform.position).normalized; 
                ram.transform.position += (newDir * landingSpeed);
                //ram.GetComponent<Rigidbody>().AddForce(newDir * landingSpeed);
            }

            if (ram.transform.position.y <= 0.75f)
            {
                if (!crashed)
                {
                    Instantiate(crashEffect, new Vector3(ram.transform.position.x, 0, ram.transform.position.z),
                                                         new Quaternion(0, -0.7056152f, -0.7085952f, 0));
                    crashEffect.SetActive(true);
                    crashed = true;
                }

                //Reenabling the Ram's components we had turned off for the meteorite effect
                ramLanding();
            }
        }
    }

    //----------------------------------------------------------------------------------------------------------|

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

        ram.GetComponentInChildren<MeshRenderer>().enabled = true;

        //Grav
        //ram.GetComponent<Rigidbody>().useGravity = false;
        
        //Freezing the rotation so that when it spawns, it keeps it's visual trajectory
        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    //Script to run as the ram hits the ground. Will switch state to the "stunned" state
    public void ramLanding()
    {

        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        ram.GetComponent<Rigidbody>().useGravity = true;
        //We want to reset it's rotation when it lands, but not along the y axis. This way it will face the same way it landed at
        ram.GetComponent<Rigidbody>().transform.position.Set(ram.transform.position.x, ram.transform.position.y + 1,
                                                                                       ram.transform.position.z);
        ram.GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, ram.transform.rotation.y, 0, 1);
        ram.GetComponent<Animator>().SetBool("isStunned", true);
    }

    // VV You need to call these 2 functions in Dynamic Camera, and add the Ram object to the list of objects it takes into account VV 
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
