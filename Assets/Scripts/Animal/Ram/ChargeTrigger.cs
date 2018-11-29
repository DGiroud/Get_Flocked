using UnityEngine;

public class ChargeTrigger : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        //We only want to look for the player
        if (other.gameObject.tag == "Player")
        {
            //If this is the same player we had just stunned, we want to ignore it
            if (other.gameObject == GetComponentInParent<Ram>().lastPlayerCharged)
            {
                return;
            }

            else
            {
                GetComponentInParent<Ram>().lastPlayerCharged = other.gameObject;

                //Here we set the Ram's gameObject reference to the player, so that it will always know where the player is when it charges
                if(other.gameObject != null)
                GetComponentInParent<Ram>().playerRef = other.gameObject;

                GetComponentInParent<Ram>().chargePlayer();
            }           
        }
    }
}
