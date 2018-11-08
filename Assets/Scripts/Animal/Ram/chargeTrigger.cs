using UnityEngine;

public class ChargeTrigger : MonoBehaviour {   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Here we set the Ram's gameObject reference to the player, so that it will always know where the player is when it charges
            GetComponentInParent<Ram>().player = other.gameObject;
            GetComponentInParent<Ram>().chargePlayer();            
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            GetComponentInParent<Ram>().chargePlayer();
        }
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
