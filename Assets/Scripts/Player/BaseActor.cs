using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ActorType
{
    Player,
    CPU
}

public class BaseActor : MonoBehaviour
{
    // actor identification labels
    [HideInInspector]
    public int actorID; // the actor's permanent ID throughout the game
    [HideInInspector]
    public ActorType actorType; // used to determine whether this is a CPU or not

    // actor movement
    #region movement
    [Header("Movement")]
    public bool canMove = true; // cause why not, right?
    public float speed = 1.0f; // how fast player go
    private float originalSpeed;
    private Vector3 translation;
    private Vector3 lastPosition;
    private CharacterController controller;
    [HideInInspector]
    public bool stunned;    // Jake's touching your code ♣
    private float stunnedTimer = 0f;
    #endregion

    // actor interaction
    #region snapping
    [Header("Snapping")]

    [SerializeField]
    private bool canSnap = true;
    [SerializeField]
    private BoxCollider interactionBox; // hit box in front of player
    public GameObject interactionSheep; // holds a sheep if there is one in front of the player
    private GameObject heldSheep; // null if no sheep, sheep if sheep
    public GameObject HeldSheep { get { return heldSheep; } }
    [SerializeField]
    private float pickUpDelay;
    private float pickUpTimer;
    #endregion

    // actor kick
    #region kicking
    [Header("Kicking")]

    [SerializeField]
    private bool canKick = true;
    public float kickHeight = 5.0f; //kick height of sheep
    public float kickForce = 5.0f; //how much force given when sheep is kicked
    #endregion


    public Vector3 moveDir;
    public float maxDash = 2.0f;
    public float minDash = 2.0f;
    public float stopDash = 2.0f;
    private float dashTime;


    /// <summary>
    /// Ensures all relevant actor variables are reset upon start-up
    /// </summary>
    void Awake ()
    {
        pickUpTimer = 0.0f;
        heldSheep = null;

        SetCanMove(canMove);
        SetCanSnap(canSnap);
        SetCanKick(canKick);

        originalSpeed = speed;
        controller = GetComponent<CharacterController>();
    }

    void Start()
    {
        dashTime = maxDash;
    }

    /// <summary>
    /// (Right now) only increments the pick-up buffer timer
    /// </summary>
    public virtual void Update ()
    {
        pickUpTimer += Time.deltaTime;

        if (stunned)
            StunTimer();
    }
    
    /// <summary>
    /// helper function which toggles the players mobility
    /// </summary>
    /// <param name="toggle">pass in true/false to toggle movement on/off</param>
    public void SetCanMove(bool toggle)
    {
        canMove = toggle;
    }

   

    /// <summary>
    /// handles player rotation and translation
    /// </summary>
    /// <param name="xAxis">pass in an x axis value between -1 and 1</param>
    /// <param name="yAxis">pass in a y axis value between -1 and 1</param>
    public void Move(float xAxis, float yAxis)
    {
        if (!canMove)
            return; // can't move!

        if (stunned)
            return; //Jake did this lmao

        translation.x = xAxis;
        translation.z = yAxis;

        // rotation handling
        transform.rotation = Quaternion.LookRotation(translation);

        lastPosition = transform.position;

        // perform movement
        controller.SimpleMove(translation * speed);

        // calculate and increment distance travelled
        float distanceTravelled = Vector3.Distance(transform.position, lastPosition);
        ScoreManager.Instance.AddDistanceTravelled(actorID, distanceTravelled);

    }

    /// <summary>
    /// helper function which toggles the players interactablity
    /// </summary>
    /// <param name="toggle">pass in true/false to toggle interaction on/off</param>
    public void SetCanSnap(bool toggle)
    {
        canSnap = toggle;
        interactionBox.enabled = toggle;
        pickUpTimer = 0.0f;
    }

    /// <summary>
    /// performs snapping of sheep by adjusting position, parenting the sheep
    /// to this actor and disabling rigidbody
    /// </summary>
    /// <param name="sheep">the desired sheep to snap to actor</param>
    public void SnapSheep(GameObject sheep)
    {
        // if a sheep was just let go of, disallow snapping for a short time
        if (pickUpTimer < pickUpDelay)
            return;

        // disable snapping
        SetCanSnap(false);

        // adjust position
        heldSheep = sheep; // update sheep reference

        Sheep sheepScript = heldSheep.GetComponent<Sheep>();
        sheepScript.SetPushedTrue();
        Animator sheepAnimation = heldSheep.GetComponentInChildren<Animator>();
        sheepAnimation.SetBool("isPushed", true);
        sheepAnimation.SetBool("isKicked", false);
        sheepAnimation.SetBool("isWandering", false);


        //                          position              direction                         offset
        Vector3 snapPosition = transform.position + translation.normalized * (sheepScript.radius * 2.1f);
        snapPosition.y += transform.localScale.y;
        heldSheep.transform.position = snapPosition;
        heldSheep.transform.SetParent(transform); // player now parents sheep

        // freeze sheep position and turn it into a trigger such that it doesn't collide 
        // with anything but the goals
        heldSheep.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePosition;
        heldSheep.GetComponent<SphereCollider>().isTrigger = true;

        // modify player speed when carrying heavy sheep
        speed *= sheepScript.speedModifier;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="toggle"></param>
    public void SetCanKick(bool toggle)
    {
        canKick = toggle;
    }

    /// <summary>
    /// performs the "detaching" of sheep. Simply unparents the sheep from
    /// this actor and re-enables physics
    /// </summary>
    /// <returns>returns the released sheep, or null if no sheep was held</returns>
    public GameObject ReleaseSheep()
    {
        // safety check: if no sheep is held, then can't release nothing
        if (!heldSheep)
            return null;

        SetCanSnap(true);

        // get held sheep
        GameObject releasedSheep = heldSheep;
        heldSheep = null; // delete reference

        Sheep sheepScript = releasedSheep.GetComponent<Sheep>();
        sheepScript.SetKickedTrue();

        Animator sheepAnimation = releasedSheep.GetComponentInChildren<Animator>();
        sheepAnimation.SetBool("isKicked", true);
        sheepAnimation.SetBool("isPushed", false);
        sheepAnimation.SetBool("isWandering", false);


        // release sheep child from this
        releasedSheep.transform.SetParent(null);

        // unfreeze position and give the sheep it's collision back
        releasedSheep.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.None;
        releasedSheep.GetComponent<SphereCollider>().isTrigger = false;

        // revert player speed to original speed
        speed = originalSpeed;

        return releasedSheep; // return for convenience sake
    }

    /// <summary>
    /// Performs the lobbing of the given sheep in a projectile motion
    /// </summary>
    /// <param name="sheep">the desired sheep to launch</param>
    
    public void LaunchSheep(GameObject sheep)
    {
        Animator sheepAnimator = sheep.GetComponent<Animator>(); //script
        sheepAnimator.SetBool("isKicked", true);
        sheepAnimator.SetBool("isPushed", false);
        sheepAnimator.SetBool("isWandering", false);

        Animator sheepAnimation = sheep.GetComponentInChildren<Animator>();
        sheepAnimation.SetBool("isKicked", true);
        sheepAnimation.SetBool("isPushed", false);
        sheepAnimation.SetBool("isWandering", false);

        Rigidbody sheepRigidbody = sheep.GetComponent<Rigidbody>(); //rb
        
        //Giving the player kick force and the sheep some height when kicked
        Vector3 kickVector = translation.normalized * kickForce;
        kickVector.y = kickHeight;

        //Adding instant kick force
        sheepRigidbody.AddForce(kickVector, ForceMode.Impulse);
    }

    /// <summary>
    /// checks whether the sheep is parented to a player, and if so, releases
    /// it from the player and launches it
    /// </summary>
    /// <param name="sheep"></param>
    public void LaunchOpponentsSheep(GameObject sheep)
    {
        if (sheep.transform.parent.CompareTag("Player"))
        {
            BaseActor opponentScript = sheep.GetComponentInParent<BaseActor>();
            
            LaunchSheep(opponentScript.ReleaseSheep());
        }
    }

    public void StunTimer()
    {
        stunnedTimer += Time.deltaTime;
        
        if(stunnedTimer >= 4)   //Hardcoded 4 because running low on time for the day
        {
            stunned = false;
        }
    }
}
