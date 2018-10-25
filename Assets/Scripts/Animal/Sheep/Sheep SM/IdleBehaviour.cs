using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleBehaviour : StateMachineBehaviour {

    GameObject sheep;
    private float timer = 0f;


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

        LeashSheep();
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep.GetComponent<Sheep>().SetWanderTrue();
    }

    void LeashSheep()
    {
        float spawnForce = Random.Range(sheep.GetComponent<Sheep>().spawnRangeForce.x, sheep.GetComponent<Sheep>().spawnRangeForce.y);

        if (sheep.transform.position.x <= GameObject.Find("Invisible Wall Left").transform.position.x)
            sheep.GetComponent<Rigidbody>().AddForce(spawnForce, 0, 0, ForceMode.Force);
        else if (sheep.transform.position.x >= GameObject.Find("Invisible Wall Right").transform.position.x)
            sheep.GetComponent<Rigidbody>().AddForce(-spawnForce, 0, 0, ForceMode.Force);
        else if (sheep.transform.position.z <= GameObject.Find("Invisible Wall Bottom").transform.position.z)
            sheep.GetComponent<Rigidbody>().AddForce(0, 0, spawnForce, ForceMode.Force);
        else if (sheep.transform.position.z >= GameObject.Find("Invisible Wall Top").transform.position.z)
            sheep.GetComponent<Rigidbody>().AddForce(0, 0, -spawnForce, ForceMode.Force);
    }
}
