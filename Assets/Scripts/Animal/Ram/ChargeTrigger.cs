using UnityEngine;

public class ChargeTrigger : MonoBehaviour {   

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Here we set the Ram's gameObject reference to the player, so that it will always know where the player is when it charges
            GetComponentInParent<Ram>().player = other.gameObject;

            //We don't want to continuously charge the same player, so we give the players a soft immunity if they'd already been charged
            if (!other.GetComponent<Player>().isStunned)
            {
                Debug.Log("not stunned");
                //Change state to continue the behavioural lööp, bröther
                GetComponentInParent<Ram>().chargePlayer();
            }
            else
            {
                Debug.Log("stunned");
                return;
            }

            ////If the player in our sphere is the same player as the last one we charged AND the cooldown hasn't elapsed, continue wandering
            //else if (GetComponentInParent<Ram>().IsSamePlayer(other.gameObject) && !GetComponentInParent<Ram>().CooldownCheck())
            //{
            //    GetComponentInParent<Animator>().SetBool("isWandering", true);
            //}

            ////If the player in our sphere is the same player as teh last one we charged AND the cooldown HAS elapsed, charge that player $$$
            //else if (GetComponentInParent<Ram>().IsSamePlayer(other.gameObject) && !GetComponentInParent<Ram>().CooldownCheck())
            //{
            //    //Change state to continue the behavioural lööp, bröther
            //    GetComponentInParent<Ram>().chargePlayer();
            //}
        }
    }

    //EACH PLAYER SHOULD HAVE AN "ISF" BOOL, Wherin if the player is indeed stunned, the Ram will ignore them completely in it's 

    private void OnTriggerStay(Collider other)
    {
       
    }

    private void OnTriggerExit(Collider other)
    {

    }
}
