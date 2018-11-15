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
        //******************************************
        //Set animator for charge preparation here *
        //******************************************

        //Initialising our player and ram gameObjects for easier accessibility throughout this script
        player = animator.GetComponent<Ram>().player;
     
        ram = animator.GetComponent<Ram>();

        //Disable the chargeSphere, we don't want the Ram to check for new player's whilst it's charging. This can have weird results, 
        // and simultaneously we want to enable the hitCollider so that the Ram can interact with the player when charged.
        // We want the Ram's hitCollider to be active throughout the begginning of RamStepback, through to the end of RamCharge.
        ram.GetComponent<Ram>().chargeSphere.enabled = false;
        ram.GetComponent<Ram>().hitCollider.enabled = true;

        //Get the chargeDelay value we set in our Ram prefab
        chargeDelay = animator.GetComponent<Ram>().chargeDelay;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //******************************
        //STEPBACK ANIMATION HERE
        //******************************

        timer += Time.deltaTime;

        //We want to keep the Ram looking at the player while we prepare to charge. Intimidation is key
        ram.GetComponent<Animator>().transform.LookAt(ram.player.transform);

        //After our predetermined timer has ran it's due, charge the poor fool who got too close
        if(timer >= chargeDelay)
        {
            if (ram.GetComponent<Ram>().CooldownCheck() == true)
            {
                if (ram.GetComponent<BoxCollider>().isTrigger == false)      //Error checking
                    ram.GetComponent<Animator>().SetBool("isCharging", true);
            }
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Make sure we turn this state off when we leave, otherwise we'll get weird results
        ram.GetComponent<Animator>().SetBool("isStepback", false);
    }
}
