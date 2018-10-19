using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalRadius : MonoBehaviour {

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Sheep"))
        {
            //Referencing the sheep's State Machine, we only want to interact with it if it is wandering or idling.
            // That is, we do not want to be pushing it away it if is being carried by the player
            Animator sheepBehaviour = other.GetComponent<Animator>();

            //If the sheep is wandering or idling near the goal, apply a force to move it away
            if(sheepBehaviour.GetBehaviour<WanderBehaviour>() || sheepBehaviour.GetBehaviour<IdleBehaviour>())
            {
                //Find the direction from the sheep's position the the goal's centre, and apply a force in the opposite direction
                Vector3 heading = other.transform.position - transform.position;
                other.GetComponent<Rigidbody>().AddForce(heading, ForceMode.Force);
            }
        }
    }
}
