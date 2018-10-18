using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    // actor identification labels
    [HideInInspector]
    public int actorID; // the actor's permanent ID throughout the game

    // actor movement
    #region movement
    [Header("Movement")]

    public bool canMove = true; // cause why not, right?
    public float speed = 1.0f; // how fast player go
    private float originalSpeed;
    private Vector3 translation;
    private CharacterController controller;
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
    
    /// <summary>
    /// (Right now) only increments the pick-up buffer timer
    /// </summary>
    public virtual void Update ()
    {
        pickUpTimer += Time.deltaTime;
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

        translation.x = xAxis;
        translation.z = yAxis;

        // rotation handling
        transform.rotation = Quaternion.LookRotation(translation);

        // perform movement
        controller.SimpleMove(translation * speed);
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

        //                          position              direction                         offset
        Vector3 snapPosition = transform.position + translation.normalized * (sheepScript.radius * 2.1f);
        snapPosition.y += transform.localScale.y;
        heldSheep.transform.position = snapPosition;
        heldSheep.transform.SetParent(transform); // player now parents sheep

        // disable sheep rb
        Destroy(heldSheep.GetComponent<Rigidbody>());

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

        // release sheep child from this
        releasedSheep.transform.SetParent(null);
        releasedSheep.AddComponent<Rigidbody>();

        speed = originalSpeed;

        return releasedSheep; // return for convenience sake
    }

    /// <summary>
    /// Performs the lobbing of the given sheep in a projectile motion
    /// </summary>
    /// <param name="sheep">the desired sheep to launch</param>
    
    public void LaunchSheep(GameObject sheep)
    {
        Animator sheepScript = sheep.GetComponent<Animator>(); //script
        sheepScript.SetBool("isKicked", true);

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
}
