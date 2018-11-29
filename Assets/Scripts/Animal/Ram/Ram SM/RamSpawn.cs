using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSpawn : StateMachineBehaviour {

    //This state is the Ram's intitial behaviour state, which can only be accessed when the Ram is created. 
    // This state leads into:
    //  Stunned: When the Ram has successfully landed after it's descent, it will change to it's "stunned" behaviour
    
    float timer = 0.0f;
    float spawnTimer = 0.0f;
    private float spawnTimerEnd;
    public float landingSpeed;

    private int round;          //The round we need to be at before the first Ram will spawn
    private int sheepScored;    //The amount of sheep that need to be scored before the first Ram will spawn

    private GameObject crashEffect;

    private GameObject ram;
    private GameObject ramSpawnPoint;
    private Light spotlight;

    private Vector3 ramTarget;
    private bool destination = false;
    private bool spawned     = false;
    private bool finished    = false;
    private bool crashed     = false;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialise the ram object for us to reference throughout this script
        ram = animator.gameObject;

        //Initialise the spawn points for us the reference throughout this script
        ramSpawnPoint = GameObject.Find("RamSpawnPoint");

        //ram.GetComponent<Ram>().hitCollider.enabled = false;
        ram.GetComponent<Ram>().hitTemp = ram.GetComponent<Ram>().hitCollider.transform.localScale;
        ram.GetComponent<Ram>().hitCollider.transform.localScale.Set(0.1f, 0.1f, 0.1f);

        landingSpeed = ram.GetComponent<Ram>().landingSpeed;

        //Finding the light objects within the RamManager hierarchy 
        spotlight = RamManager.Instance.GetComponentInChildren<Light>();

        //Pulling our customisable variable from the Ram prefab
        spawnTimerEnd = animator.GetComponent<Ram>().spawnTimer;

        crashEffect = ram.GetComponent<Ram>().crashEffect;

        //Setting up variables for both Ram's spawning properties
        round = ram.GetComponent<Ram>().roundSpawn;
        sheepScored = (int)Random.Range(ram.GetComponent<Ram>().sheepTilSpawn.x, ram.GetComponent<Ram>().sheepTilSpawn.y);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If conditions for the spawning of our first Ram
        if (LevelManager.GetCurrentRound() >= round)  //If we have reached the round specified, spawn Ram
        {
            if (ScoreManager.Instance.CurrentSheep >= sheepScored)  //If we have scored the specified amount of sheep, Spawn Ram
            {
                DynamicCamera.AddObjectOfInterest(ram);
                spotlight.enabled = true;
                updateSpotlight();
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        DynamicCamera.RemoveObjectOfInterest(ram);
    }

    #region Update Spotlight
    //----------------------------------------------------------------------------------------------------------|
    //                                          UPDATE SPOTLIGHT
    //  Here we pass in which spotlight we want to be updating
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

            //The instant where we crash into the ground
            if (ram.transform.position.y <= 0.75f)
            {
                if (!crashed)
                {
                    //Creating the ram, he should be facing in the same direction as he lands (spoiler: he doesn't)
                    Instantiate(crashEffect, new Vector3(ram.transform.position.x, 0.1f, ram.transform.position.z),
                                                         new Quaternion(-0.7071068f, 0, 0, 0.7071068f));
                    //Turn on the crash particle effects
                    crashEffect.SetActive(true);
                    //Make sure that we never come here again
                    crashed = true;
                }

                //Reenabling the Ram's components we had turned off for the meteorite effect
                ramLanding();
            }
        }
    }
    //----------------------------------------------------------------------------------------------------------|
    #endregion

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

        //Let's see that beaut
        ram.GetComponentInChildren<SkinnedMeshRenderer>().enabled = true;

        //Grav
        ram.GetComponent<Rigidbody>().useGravity = false;
        
        //Freezing the rotation so that when it spawns, it keeps it's visual trajectory
        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    //Script to run as the ram hits the ground. Will switch state to the "stunned" state
    public void ramLanding()
    {
        //We gotta turn off those flight constraints. My boy should be able to move organically and ram-like (◕ᴗ◕✿)
        ram.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY |
                                                    RigidbodyConstraints.FreezeRotationX |
                                                    RigidbodyConstraints.FreezeRotationZ;
        ram.GetComponent<Rigidbody>().useGravity = true;
        //We want to reset it's rotation when it lands, but not along the y axis. This way it will face the same way it landed at
        ram.GetComponent<Rigidbody>().transform.position.Set(ram.transform.position.x, ram.transform.position.y + 1,
                                                                                       ram.transform.position.z);
        ram.GetComponent<Rigidbody>().transform.rotation = new Quaternion(0, ram.transform.rotation.y, 0, 1);
        //Our little function so that we can enter the Spawn state and set the Ram into his loop
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