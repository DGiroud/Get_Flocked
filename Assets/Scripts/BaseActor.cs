using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseActor : MonoBehaviour
{
    public float speed;
    public float strength;

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
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseSheep();
        }

        pickUpTimer += Time.deltaTime;
	}

    public void SnapSheep(GameObject sheep)
    {
        // safety check: if a sheep is already held, don't hold another
        if (heldSheep)
            return;

        // if a sheep was just let go of, disallow snapping for a short time
        if (pickUpTimer < pickUpDelay)
            return;

        interactionBox.enabled = false;

        // adjust position
        sheep.transform.position = interactionBox.transform.position;
        sheep.transform.parent = transform; // player now parents sheep
        heldSheep = sheep; // update sheep reference
        heldSheep.GetComponent<Rigidbody>().isKinematic = true;
    }

    public GameObject ReleaseSheep()
    {
        // safety check: if no sheep is held, then can't release nothing
        if (!heldSheep)
            return null;

        pickUpTimer = 0.0f;
        interactionBox.enabled = true;

        // get held sheep
        GameObject releasedSheep = heldSheep;
        heldSheep = null; // delete reference

        // release sheep child from this
        releasedSheep.transform.parent = null;
        releasedSheep.GetComponent<Rigidbody>().isKinematic = false;
        return releasedSheep; // return for convenience sake
    }
}
