﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamStunned : StateMachineBehaviour {

    //This state can be accessed via:
    // Spawn: When the Ram lands from it's descent, it will change to this state.
    // Charge: Once the Ram has charged a player, it will change to this state.

    //This state leads into:
    // Wander: Once the stun timer is up, it will resume wandering.

    //Reference to the Ram game object
    GameObject ram;

    float stunDuration;
    float stunTimer = 0f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //********************************
        //Set stunned animation here

        //Initialise the ram for easier access (I.E. saves us writing "animator" all day long
        ram = animator.gameObject;

        //Use the stunDuration we've set in the Ram prefab
        stunDuration = ram.GetComponent<Ram>().stunDuration;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        stunTimer += Time.deltaTime;

        //When the Ram has been stunned for it's determined duration, start wandering
        if(stunTimer >= stunDuration)
        {
            ram.GetComponent<Animator>().SetBool("isWandering", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //We need to Set this to false when we leave, otherwise Charge will automatically send the Ram into the stunned state,
        // creating a loop
        ram.GetComponent<Animator>().SetBool("isStunned", false);
    }
}
