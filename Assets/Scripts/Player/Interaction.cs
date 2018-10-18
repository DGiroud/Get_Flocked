using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interaction : MonoBehaviour
{


    /// <summary>
    /// handle interaction box collision
    /// </summary>
    /// <param name="other">the colliding object</param>
    private void OnTriggerEnter(Collider other)
    {
        // if the colliding object isn't a sheep, ignore it
        if (!other.CompareTag("Sheep"))
            return;

        BaseActor playerScript = GetComponentInParent<BaseActor>();

        // if the sheep is already being pushed then it can't be picked up
        if (other.GetComponent<Animator>().GetBool("isPushed"))
        {
            playerScript.interactionSheep = other.gameObject;
            return;
        }

        // snap the sheep to actor
        playerScript.SnapSheep(other.gameObject);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other">the colliding object</param>
    private void OnTriggerExit(Collider other)
    {
        GetComponentInParent<BaseActor>().interactionSheep = null;
    }
}
