using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBehaviour : StateMachineBehaviour {

    GameObject sheep;
    float spawnForce;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.gameObject;

        spawnForce = Random.Range(sheep.GetComponent<Sheep>().spawnRangeForce.x, sheep.GetComponent<Sheep>().spawnRangeForce.y);

        if (sheep.transform.position.x <= GameObject.Find("Invisible Wall Left").transform.position.x)
            sheep.GetComponent<Rigidbody>().AddForce(spawnForce, 0, 0, ForceMode.Impulse);
        else if (sheep.transform.position.x >= GameObject.Find("Invisible Wall Right").transform.position.x)
            sheep.GetComponent<Rigidbody>().AddForce(-spawnForce, 0, 0, ForceMode.Impulse);
        else if (sheep.transform.position.z <= GameObject.Find("Invisible Wall Bottom").transform.position.z)
            sheep.GetComponent<Rigidbody>().AddForce(0, 0, spawnForce, ForceMode.Impulse);
        else if (sheep.transform.position.z >= GameObject.Find("Invisible Wall Top").transform.position.z)
            sheep.GetComponent<Rigidbody>().AddForce(0, 0, -spawnForce, ForceMode.Impulse);

        sheep.GetComponent<Sheep>().currentBehaviour = "Spawn Behaviour";

        sheep.GetComponent<Sheep>().SetWanderTrue();
        sheep.GetComponentInChildren<Animator>().SetBool("isWandering", true);
        sheep.GetComponentInChildren<Animator>().SetBool("isKicked", false);
        sheep.GetComponentInChildren<Animator>().SetBool("isPushed", false);
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
}
