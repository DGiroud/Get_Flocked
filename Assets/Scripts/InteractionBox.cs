using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBox : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
		
	}

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Sheep"))
        {
            BaseActor parentScript = GetComponentInParent<BaseActor>();

            parentScript.SnapSheep(other.gameObject);
        }
    }
}
