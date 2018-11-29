using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class Ram : MonoBehaviour {

    //[HideInInspector]                              //We don't want these to be accessed by anyone or anything except other scripts
    public GameObject playerRef;                      //Reference to the player we're charging
    //[HideInInspector]
    public bool playerHit = false;                 //If we hit the player, this becomes true and the player is stunned for a duration.
    //[HideInInspector]
    public bool sheepHit = false;                  //If we hit a sheep, this becomes true and the sheep gets knocked out of the way.
    //[HideInInspector]
    public bool boundaryHit = false;               //If we charge into the outer boundaries, this becomes true and we halt our charge

    //Settings to determine when and how the Rams should spawn
    #region Ram Spawn Requirements
    [Header("Spawn Requirements")]
    [Tooltip("What round the Ram will spawn after. IE if 2, the Ram will not spawn in Round 1, but will spawn in all rounds after that")]
    public int roundSpawn;
    [Tooltip("A random number will be generated between these 2 values, and when that many sheep have been scored in a round, the Ram will spawn")]
    public Vector2 sheepTilSpawn;
    [Tooltip("The duration that the Ram will wait to spawn after locking in it's location to land")]
    public float spawnTimer;
    [Tooltip("How quickly the Ram will fall into the field once it has spawned")]
    [Range(0, 1)]
    public float landingSpeed;
    #endregion

    #region Ram Properties
    [Header("Ram Properties")]
    [Tooltip("The duration that the Ram will be stunned for after charging and landing")]
    [Range(0, 10)]
    public float stunDuration;
    [Tooltip("The speed at which the Ram will wander around the playing field")]
    [Range(0, 100)]
    public float wanderSpeed;
    [Tooltip("How long the Ram waits (in seconds) before finding a new location after reaching it's original")]
    public float idleTime;
    private Vector3 pauseVelocity;  //When we pause the game, we can store the Ram's velocity in here
    private bool isPaused;

    //Wander properties
    [HideInInspector] // We want to access the field box in ramCharge
    public GameObject[] fieldBox;
    private int previousNum;
    private int rand;
    #endregion

    #region Charging Properties
    [Header("Charging Properties")]
    [Tooltip("Charge Cooldown is how long the Ram waits before charging the same player. There is no cooldown if the Ram charges 1 player," +
        "and then immediately finds another player within it's charge sphere.")]
    public float chargeCooldown;
    private float chargeCooldownTimer = 0f;
    [Tooltip("How close a player needs to be to the Ram before it charges that player")]
    public float chargeRadius = 7.5f;
    [Tooltip("How long the Ram waits before charging the player. This will need to align with the animation the Ram takes as it prepares " +
        "for it's Charge.")]
    public float chargeDelay = 2;
    [Tooltip("How long the Ram stuns the player for after successfully charging them")]
    public float playerStunTime = 3;
    private float playerStunTimer = 0f;     //Timer to count down the player's stun duration
    [Tooltip("How long the Ram must wait before charging the same player twice in a row. Gives the stunned player time to escape the Ram")]
    public float samePlayerCooldown;
    private float samePlayerTimer = 0f;     //Timer to reference against the samePlayerCooldown
    public GameObject lastPlayerCharged;    //Reference for the ChargeTrigger, so that we can ignore
    [HideInInspector]
    private ChargeTrigger chargeTrigger;
    #endregion    

    #region Particles and Colliders
    //References to our different particle effects
    public GameObject crashEffect;      //Ram landing into the scene
    public GameObject chargeEffect;     //Ram hitting a target after charging
    public GameObject stunnedEffect;    //Ram stunning a player
    [HideInInspector]
    public Vector3 chargeTemp;       //Turning colliders on and off causes issues, so instead we just shrink them temporarily
    [HideInInspector]
    public Vector3 hitTemp;

    [Header("Ram Colliders")]
    [Tooltip("Sphere collider that the Ram uses during it's initial descent, knocks all actors away if they come too close")]
    public SphereCollider meteorSphere;

    [Tooltip("Sphere collider with a large radius that checks if a player has entered the Ram's range or not, and if so," +
        " sends the Ram into it's charge state directed towards that player")]
    public SphereCollider chargeSphere;

    [Tooltip("Box collider that becomes active whilst the Ram is charging, interacts with players and sheep if they are hit by the Ram")]
    public BoxCollider hitCollider;

   
    #endregion


    private void Start () {
        //Creating the FieldBox array;
        fieldBox = new GameObject[4];

        //Initialising each element of our field array to the 4 field objects in our scene
        fieldBox[0] = GameObject.Find("FieldTop");
        fieldBox[1] = GameObject.Find("FieldRight");
        fieldBox[2] = GameObject.Find("FieldBottom");
        fieldBox[3] = GameObject.Find("FieldLeft");

        chargeTrigger = GetComponentInChildren<ChargeTrigger>();
    }

    public void Update()
    {
        chargeCooldownTimer += Time.deltaTime;

        if(playerHit == true)
        {
            //if(playerRef == null)
            //{
            //    return;
            //}

            playerStunTimer += Time.deltaTime;

            if(playerStunTimer >= playerStunTime)
            {
                //Once the player has been stunned for the desired duration, reenable their movement and reset our variables
                //player.GetComponent<Player>().isStunned = false;
                playerRef.GetComponent<BaseActor>().stunned = false;

                playerRef = null;
                playerHit = false;
                playerStunTimer = 0f;
            }
        }

        //if (boundaryHit == true)
        //    playerRef = null;

        if(Time.timeScale == 0 && isPaused == false)
        {
            isPaused = true;
            pauseVelocity = GetComponent<Rigidbody>().velocity;
            GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        }

        if (isPaused == true && Time.timeScale != 0)
        {
            GetComponent<Rigidbody>().velocity = pauseVelocity;
            isPaused = false;
        }

        ////If the Ram has charged someone, we want there to be a cooldown before he can charge them again
        //if (lastPlayerCharged != null)
        //{
        //    samePlayerTimer += Time.deltaTime;

        //    if(samePlayerTimer >= samePlayerCooldown)
        //    {
        //        samePlayerTimer = 0;

        //        lastPlayerCharged = null;
        //    }
        //}

        //else if()

        //If the ram somehow finds it's wya off the field, destroy it so that it doesn't mess with the dynamic camera
        if (transform.position.y <= -10)
        {
            Destroy(GetComponent<Ram>());
        }
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
    //thank u mr ram

    public void chargePlayer()
    {
        //Turn off our charging sphere so that we don't get issues with finding another player mid-charge
        chargeSphere.enabled = false;

        //This is called from the Ram prefab's chargeRadius sphere on TriggerEnter.
        // Will immediately change the Ram's behaviour to begin it's charging sequence
        GetComponent<Animator>().SetBool("isStepback", true);
    }

    //Function to help us check i fwe are trying to charge the same player
    public bool IsSamePlayer(GameObject inPlayer)
    {
        if (inPlayer == playerRef)
            return true;

        else return false;
    }

    public bool CooldownCheck()
    {
        if (chargeCooldownTimer >= chargeCooldown)
            return true;

        else return false;
    }
}