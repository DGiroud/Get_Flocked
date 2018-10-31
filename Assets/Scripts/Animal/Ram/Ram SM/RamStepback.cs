using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamStepback : StateMachineBehaviour {

    //This state, whilst small, is responsible for the visual stepback or "kicking feet" that the Ram will do before charging a sighted player
    // In this state, the Ram will continually look towards the player, and slowly move backwards as a "wind-up", before entering the Charge.

   

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
