using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KickBehaviour : StateMachineBehaviour {

    GameObject sheep;
    float timer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.gameObject;

        sheep.GetComponent<Sheep>().currentBehaviour = "Kick Behaviour";
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        timer += Time.deltaTime;

        if (sheep.GetComponent<Sheep>().IsGrounded())
        {
            if (timer >= 5)
            {
                sheep.GetComponent<Sheep>().SetIdleTrue();
                sheep.GetComponentInChildren<Animator>().SetBool("isWandering", false);
                sheep.GetComponentInChildren<Animator>().SetBool("isKicked", false);
                sheep.GetComponentInChildren<Animator>().SetBool("isPushed", false);
            }
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
