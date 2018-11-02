using UnityEngine;

public class ChargeTrigger : MonoBehaviour {   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
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
