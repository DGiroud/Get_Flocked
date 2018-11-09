using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour {

    GameObject sheep;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.gameObject;

        sheep.GetComponent<Sheep>().currentBehaviour = "Idle Behaviour";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If the sheep has remained idle for a specified amount of time, seek a new location       
        sheep.GetComponent<Sheep>().SetWanderTrue();
        sheep.GetComponentInChildren<Animator>().SetBool("isWandering", true);
        sheep.GetComponentInChildren<Animator>().SetBool("isKicked", false);
        sheep.GetComponentInChildren<Animator>().SetBool("isPushed", false);
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep.GetComponent<Sheep>().SetWanderTrue();
        sheep.GetComponentInChildren<Animator>().SetBool("isWandering", true);
        sheep.GetComponentInChildren<Animator>().SetBool("isKicked", false);
        sheep.GetComponentInChildren<Animator>().SetBool("isPushed", false);
    }
}
