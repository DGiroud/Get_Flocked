using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour {

    //This is the collider that the Ram will use during it's charge towards a player - and only then

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //We want different reactions for when the Ram hits either a player or a sheep
        {
            if (other.gameObject != GetComponentInParent<Ram>().player)
                GetComponentInParent<Ram>().player = other.gameObject;

            //If the player we hit is different from the player we were charging, we want to change the Ram's player reference so that 
            // we don't stun the wrong player
            if (other.gameObject != GetComponentInParent<Ram>().player)
                GetComponentInParent<Ram>().player = other.gameObject;

            //Enables the stun functionality in the Ram script
            GetComponentInParent<Ram>().playerHit = true;

            //If the player is holding a sheep, make them drop it
            if(other.GetComponent<BaseActor>().HeldSheep)
            other.GetComponent<BaseActor>().ReleaseSheep();
        }

        else if (other.gameObject.tag == "Boundary")
        {
            //Enables the boundary hit functionality in the Ram script
            GetComponentInParent<Ram>().boundaryHit = true;
        }

        //Note, we do not need a case for sheep here, as they are simply pushed out of the way if they get in the way of the Ram
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {
        
    }
}