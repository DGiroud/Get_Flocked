using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamStunned : StateMachineBehaviour {

    //This state can be accessed via:
    // Spawn: When the Ram lands from it's descent, it will change to this state.
    // Charge: Once the Ram has charged a player, it will change to this state.

    //This state leads into:
    // Wander: Once the stun timer is up, it will resume wandering.

    float stunDuration;
    float stunTimer = 0f;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Set stunned animation here
        
        stunDuration = animator.GetComponent<Ram>().stunDuration;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

        stunTimer += Time.deltaTime;


        //When the Ram has been stunned for it's determined duration, start wandering
        if(stunTimer >= stunDuration)
        {
            animator.SetBool("isWandering", true);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
