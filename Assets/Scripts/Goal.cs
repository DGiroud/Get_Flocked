using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    /// <summary>
    /// handles the 
    /// </summary>
    /// <param name="other">the object which passed through the goals</param>
    private void OnTriggerEnter(Collider other)
    {
        // if the collided object is a sheep
        if (other.CompareTag("Sheep"))
        {
            BaseActor parentScript = other.GetComponentInParent<BaseActor>();

            // if the sheep is still parented to an actor...
            if (parentScript)
                parentScript.ReleaseSheep(); //... release it

            // destroy sheep
            AnimalManager.Instance.DestroySheep(other.gameObject);
        }
    }
}
