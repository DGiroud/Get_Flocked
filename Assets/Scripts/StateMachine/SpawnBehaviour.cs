using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : StateMachineBehaviour {

    GameObject sheep;
    float timer = 0.0f;
    float spawnForce;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.gameObject;

        spawnForce = Random.Range(sheep.GetComponent<Sheep>().spawnRangeForce.x, sheep.GetComponent<Sheep>().spawnRangeForce.y);

        if (sheep.transform.position.x <= -16)
            sheep.GetComponent<Rigidbody>().AddForce(spawnForce, 0, 0, ForceMode.Impulse);
        else if (sheep.transform.position.x >= 16)
            sheep.GetComponent<Rigidbody>().AddForce(-spawnForce, 0, 0, ForceMode.Impulse);
        else if (sheep.transform.position.z <= -16)
            sheep.GetComponent<Rigidbody>().AddForce(0, 0, spawnForce, ForceMode.Impulse);
        else if (sheep.transform.position.z >= 16)
            sheep.GetComponent<Rigidbody>().AddForce(0, 0, -spawnForce, ForceMode.Impulse);

        sheep.GetComponent<Sheep>().currentBehaviour = "Spawn Behaviour";

        sheep.GetComponent<Sheep>().SetWanderTrue();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //timer += Time.deltaTime;

        //if(timer >= 2.5f)
        //As soon as the sheep is reset, we use this state to reset all of it's variables before moving it straight into it's wander state
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

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
