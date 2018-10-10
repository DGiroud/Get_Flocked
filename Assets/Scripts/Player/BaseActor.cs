using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    // actor identification labels
    [HideInInspector]
    public int actorID; // the actor's permanent ID throughout the game
    [HideInInspector]
    public int spawnID; // the actor's ID for goals and spawning

    // actor movement
    [Header("Movement")]
    [SerializeField]
    private bool canMove = true; // cause why not, right?
    public float speed; // how fast player go
    private Rigidbody playerRigidbody; // reference to rb
    private Vector3 translation;

    // actor interaction
    [Header("Interaction")]
    [SerializeField]
    private bool canInteract = true;
    [SerializeField]
    private BoxCollider interactionBox; // hit box in front of player
    private GameObject heldSheep; // null if no sheep, sheep if sheep
    [SerializeField]
    private float pickUpDelay;
    private float pickUpTimer;

    /// <summary>
    /// Ensures all relevant actor variables are reset upon start-up
    /// </summary>
	void Awake ()
    {
        pickUpTimer = 0.0f;
        heldSheep = null;

        SetCanInteract(canInteract);
        SetCanMove(canMove);

        playerRigidbody = GetComponentInParent<Rigidbody>();
    }
    
    /// <summary>
    /// (Right now) only increments the pick-up buffer timer
    /// </summary>
    public virtual void Update ()
    {
        pickUpTimer += Time.deltaTime;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="xAxis"></param>
    /// <param name="yAxis"></param>
    public void Move(float xAxis, float yAxis)
    {
        if (!canMove)
            return; // can't move!

        translation.x = xAxis;
        translation.z = yAxis;

        // multiply by speed and delta time
        translation *= speed * Time.deltaTime;

        // rotation handling
        playerRigidbody.MoveRotation(Quaternion.LookRotation(translation));

        // perform movement
        playerRigidbody.MovePosition(transform.position + translation);
    }

    /// <summary>
    /// handle interaction box collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // if the object is a sheep...
        if (other.transform.parent.CompareTag("Sheep"))
        {
            // ...snap the sheep to actor
            SnapSheep(other.transform.parent.gameObject);
        }
    }

    public void SetCanInteract(bool toggle)
    {
        canInteract = toggle;
        interactionBox.enabled = toggle;
    }

    public void SetCanMove(bool toggle)
    {
        canMove = toggle;
    }

    /// <summary>
    /// performs snapping of sheep by adjusting position, parenting the sheep
    /// to this actor and disabling rigidbody
    /// </summary>
    /// <param name="sheep">the desired sheep to snap to actor</param>
    public void SnapSheep(GameObject sheep)
    {
        // safety check: if a sheep is already held, don't hold another
        if (heldSheep)
            return;

        // if a sheep was just let go of, disallow snapping for a short time
        if (pickUpTimer < pickUpDelay)
            return;

        // disable the actor's trigger box
        interactionBox.enabled = false;

        // adjust position
        heldSheep = sheep; // update sheep reference

        Sheep sheepScript = heldSheep.GetComponent<Sheep>();
        sheepScript.SetState(Sheep.SheepState.Push);

        //                          position              direction                         offset
        Vector3 snapPosition = transform.position + translation.normalized * (sheepScript.CurrentTier.size);

        heldSheep.transform.position = snapPosition;
        heldSheep.transform.SetParent(transform); // player now parents sheep

        // disable sheep
        heldSheep.GetComponent<Rigidbody>().isKinematic = true;
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

        // enable actor's trigger box
        interactionBox.enabled = true; 
        pickUpTimer = 0.0f; // reset timer

        // get held sheep
        GameObject releasedSheep = heldSheep;
        heldSheep = null; // delete reference

        Sheep sheepScript = releasedSheep.GetComponentInParent<Sheep>();
        sheepScript.SetState(Sheep.SheepState.Idle);

        // release sheep child from this
        releasedSheep.transform.SetParent(null);
        releasedSheep.GetComponentInParent<Rigidbody>().isKinematic = false;

        return releasedSheep; // return for convenience sake
    }

    /// <summary>
    /// Performs the lobbing of the given sheep in a projectile motion
    /// </summary>
    /// <param name="sheep">the desired sheep to launch</param>
    public void LaunchSheep(GameObject sheep)
    {
        //Sheep sheepScript = sheep.GetComponentInParent<Sheep>();
        //sheepScript.SetState(Sheep.SheepState.Kick);
    }
}
