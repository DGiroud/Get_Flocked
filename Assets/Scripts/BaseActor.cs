using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    [Header("Movement")]
    public float speed;
    public float strength;

    [Header("Interaction")]
    public BoxCollider interactionBox;
    public float pickUpDelay;
    private float pickUpTimer = 0.0f;
    private GameObject heldSheep = null;

	// Use this for initialization
	void Start ()
    {
		
	}

    // Update is called once per frame
    public virtual void Update ()
    {
        pickUpTimer += Time.deltaTime;
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
        sheep.transform.position = interactionBox.transform.position;
        sheep.transform.parent = transform; // player now parents sheep
        heldSheep = sheep; // update sheep reference
        heldSheep.GetComponent<Rigidbody>().isKinematic = true;
    }

    /// <summary>
    /// performs the "detaching" of sheep. Simple unparents the sheep from
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

        // release sheep child from this
        releasedSheep.transform.parent = null;
        releasedSheep.GetComponent<Rigidbody>().isKinematic = false;
        return releasedSheep; // return for convenience sake
    }
}
