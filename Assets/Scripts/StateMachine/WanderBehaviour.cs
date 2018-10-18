using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : StateMachineBehaviour {

    GameObject sheep;
    Transform sheepPos;

    Vector3 newPos;
    float destCheckRadius;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.gameObject;
        sheepPos = sheep.GetComponent<Transform>();

        sheep.GetComponent<Sheep>().currentBehaviour = "Wander Behaviour";

        //Here we find a new position to seek towards when the object is created
        newPos = sheep.GetComponent<Sheep>().GetNewDestination();
        destCheckRadius = sheep.GetComponent<Sheep>().destCheckRadius;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Position and rotation updates
        sheep.GetComponent<Rigidbody>().AddRelativeForce(newPos, ForceMode.Force);

        //Every frame we check whether the sheep is within an acceptable range or not, wherein we change to our Idle state
        if ((sheepPos.position.x <= newPos.x + destCheckRadius || sheepPos.position.x >= newPos.x - destCheckRadius) ||
           (sheepPos.position.z <= newPos.z + destCheckRadius || sheepPos.position.z >= newPos.z - destCheckRadius))
        {
            animator.SetBool("isWandering", false);
        }

        //Check if outside field
        if(sheep.GetComponent<Sheep>().CheckOutsideField(sheep) == true)
        {
            sheep.GetComponent<Rigidbody>().AddForce(new Vector3(0, 0, 0), ForceMode.Force);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("isWandering", false);
    }

    // OnStateMove is called right after Animator.OnAnimatorMove(). Code that processes and affects root motion should be implemented here
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK(). Code that sets up animation IK (inverse kinematics) should be implemented here.
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
    //
    //}
}
