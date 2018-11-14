using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitTrigger : MonoBehaviour {

    //This is the collider that the Ram will use during it's charge towards a player - and only then

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")   //We want different reactions for when the Ram hits either a player or a sheep
        {
            GetComponentInParent<Ram>().playerHit = true;
        }

        else if (other.gameObject.tag == "Sheep")
        {
            //TO-DO LIST:
            // If a sheep happens to be in the way of the ram, we want to knock it back so it doesn't interfere with the movement of the Ram
            // Could potentially bring back the meteor trigger for this, as that would also affect players that AREN'T being targeted by the
            //  Ram during it's charge, covering all bases of potential errors

            GetComponentInParent<Ram>().sheepHit = true;
        }
    }

    public void OnTriggerStay(Collider other)
    {

    }

    public void OnTriggerExit(Collider other)
    {
    }
}