using UnityEngine;

public class ChargeTrigger : MonoBehaviour {

    public GameObject lastPlayerCharged;
    public bool stopIgnoring;

    private void OnTriggerEnter(Collider other)
    {
        //We only want to look for the player
        if (other.gameObject.tag == "Player")
        {
            //If this is the same player we had just stunned, we want to ignore it
            if (other.gameObject == lastPlayerCharged)
            {
                //Handy little trick to completely ignore a collider
                Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }

            //*******************
            // Potential issue here, the function above sets the player to be ignored, yet the one underneat h reverts that the logic
            // throughout the rest of the code is flawed in a way that sometimes these are both called, rendering one useless
            //*******************

            //PLAYER BEING IGNORED
            //If we've triggered the event in Ram to stop ingoring the lastPlayerCharged, we want to enable that here.
            if (stopIgnoring == true)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }

            else
            {
                lastPlayerCharged = other.gameObject;

                //Here we set the Ram's gameObject reference to the player, so that it will always know where the player is when it charges
                GetComponentInParent<Ram>().player = other.gameObject;
                GetComponentInParent<Ram>().chargePlayer();
            }           
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //If this is the same player we had just stunned, we want to ignore it
            if (other.gameObject == lastPlayerCharged)
            {
                //Handy little trick to completely ignore a collider
                Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>(), true);
            }

            else
            {
                lastPlayerCharged = other.gameObject;

                //Here we set the Ram's gameObject reference to the player, so that it will always know where the player is when it charges
                GetComponentInParent<Ram>().player = other.gameObject;
                GetComponentInParent<Ram>().chargePlayer();
            }

            //PLAYER BEING IGNORED
            //If we've triggered the event in Ram to stop ingoring the lastPlayerCharged, we want to enable that here.
            if (stopIgnoring == true)
            {
                Physics.IgnoreCollision(other.GetComponent<Collider>(), GetComponent<Collider>(), false);
            }            
        }
    }
}
