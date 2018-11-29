using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamStepback : StateMachineBehaviour {

    //This state, whilst small, is responsible for the visual stepback or "kicking feet" that the Ram will do before charging a sighted player
    // In this state, the Ram will continually look towards the player, and slowly move backwards as a "wind-up", before entering the Charge.

    float chargeDelay;
    float timer;
    GameObject player;
    Ram ram;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialising our player and ram gameObjects for easier accessibility throughout this script
        player = animator.GetComponent<Ram>().playerRef;
     
        ram = animator.GetComponent<Ram>();

        ram.GetComponent<Rigidbody>().velocity = new Vector3(0, 0, 0);
        ram.GetComponent<Rigidbody>().isKinematic = true;

        //Disable the chargeSphere, we don't want the Ram to check for new player's whilst it's charging. This can have weird results, 
        // and simultaneously we want to enable the hitCollider so that the Ram can interact with the player when charged.
        // We want the Ram's hitCollider to be active throughout the begginning of RamStepback, through to the end of RamCharge.
        ram.chargeTemp = ram.chargeSphere.transform.localScale;
        ram.chargeSphere.transform.localScale.Set(0.1f, 0.1f, 0.1f);       
        

        //Get the chargeDelay value we set in our Ram prefab
        chargeDelay = animator.GetComponent<Ram>().chargeDelay;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        //If our player reference exists, continue as normal
        if (ram.GetComponent<Ram>().playerRef)
        {
            //We want to keep the Ram looking at the player while we prepare to charge. Intimidation is key
            ram.GetComponent<Animator>().transform.LookAt(ram.playerRef.transform);

            //After our predetermined timer has ran it's due, charge the poor fool who got too close
            if (timer >= chargeDelay)
            {
                if (ram.GetComponent<Ram>().CooldownCheck() == true)
                {
                    if (ram.GetComponent<BoxCollider>().isTrigger == false)      //Error checking
                    {
                        timer = 0;

                        ram.GetComponent<Animator>().SetBool("isCharging", true);
                    }
                }
            }
        }

        //If the player reference is null, something has gone wrong during StepBack and we move back into our Wander state;
        else
            ram.GetComponent<Animator>().SetBool("isWandering", true);
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If we move from StepBack back into Wander, then we need to reset this back to false to continue the behavioural loop
        if (ram.GetComponent<Ram>().boundaryHit == true)
            ram.GetComponent<Ram>().boundaryHit = false;

        ram.GetComponent<Ram>().chargeSphere.transform.localScale.Set(ram.chargeTemp.x, ram.chargeTemp.y, ram.chargeTemp.z);
        ram.GetComponent<Ram>().hitCollider.transform.localScale.Set(ram.hitTemp.x, ram.hitTemp.y, ram.hitTemp.z);

        //Make sure we turn this state off when we leave, otherwise we'll get weird results
        ram.GetComponent<Animator>().SetBool("isStepback", false);
    }
}
