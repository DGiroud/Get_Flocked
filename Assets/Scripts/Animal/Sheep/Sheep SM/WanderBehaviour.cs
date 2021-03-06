﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderBehaviour : StateMachineBehaviour {

    GameObject sheep;
    GameObject goalRadius;
    Transform sheepPos;
    float sheepSpeed;

    Vector3 newPos = new Vector3();

    Vector3[] currentPath;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        sheep = animator.gameObject;
        sheepPos = sheep.GetComponent<Transform>();
        sheepSpeed = sheep.GetComponent<Sheep>().speed * sheep.GetComponent<Sheep>().speedModifier;

        //Finding each of the radius' surrounding the goals, where the sheep will be forced away from should they enter the perimeter 
        goalRadius = GameObject.FindWithTag("GoalRadius");

        //Here we find a new position to seek towards when the object is created
        newPos = sheep.GetComponent<Sheep>().GetNewDestination();

    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 newDir = new Vector3();
        
        currentPath = PathManager.Instance.FindPath(sheep.transform.position, newPos);

        ////Work out direction (Destination - current)
        if(currentPath != null && currentPath.Length > 0)
        {
            newDir = (currentPath[1] - currentPath[0]).normalized;
        }

        //Add force using direction * speed
        if(sheep.GetComponent<Rigidbody>())
        sheep.GetComponent<Rigidbody>().AddForce((newDir * sheepSpeed) * Time.deltaTime, ForceMode.Force);

        //Every frame we check whether the sheep is within an acceptable range or not, wherein we change to our Idle state
        if (sheep.transform.position.x >= newPos.x - 1 && sheep.transform.position.x <= newPos.x + 1 &&
           sheep.transform.position.z >= newPos.z - 1 && sheep.transform.position.z <= newPos.z + 1)
        {
            newPos = sheep.GetComponent<Sheep>().GetNewDestination();
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }

    //This function helps to keep the sheep INSIDE the play area, by applying an opposing force when they try to leave.
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
