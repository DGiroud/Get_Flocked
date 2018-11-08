﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamWander : StateMachineBehaviour {

    //This state is responsible for the Ram wandering around the field looking for players to charge.
    //This state can be access via:
    // Stunned: After the Ram has been stunned, it will resume wandering in this state after completing it's stun duration
    // Charge: If the Ram charges a player and misses, it will not be stunned, but instead resume wandering.

    //This state leads into:
    // Stepback: Once a player enter's the Ram's danger zone (lana), then it will enter it's Stepback state;

    //Reference to the Ram game object
    GameObject ram;

    //The position that the Ram will seek to
    Vector3 newPos = new Vector3();
    Vector3 newDir = new Vector3();
    float movementSpeed;

    //Pathfinding variables
    NavMeshAgent navMesh;
    Vector3[] currentPath;
    private float timer;
    private float idleTimer;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialising ram for easier access throughout this script
        ram = animator.gameObject;
        //Finding a new position to Wander to
        newPos  = ram.GetComponent<Ram>().GetNewDestination();
        navMesh = ram.GetComponent<NavMeshAgent>();
        movementSpeed = ram.GetComponent<Ram>().wanderSpeed;
        idleTimer = ram.GetComponent<Ram>().idleTime;

        ram.GetComponent<Ram>().meteorSphere.enabled = false;
        ram.GetComponent<Ram>().chargeSphere.enabled = true;
        ram.GetComponent<Ram>().chargeSphere.radius = ram.GetComponent<Ram>().chargeRadius;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (navMesh.enabled == false)
            navMesh.enabled = true;

        currentPath = PathManager.Instance.FindPath(ram.transform.position, newPos); //Where the path is being calculated


        //move towards new location
        //Work out direction (Destination - current)
        if (currentPath != null && currentPath.Length > 0)
        {
            newDir = (currentPath[1] - currentPath[0]).normalized;            
        }

        //Drawing the path
        PathManager.Instance.DrawPath(currentPath);

        //We need to ditch the idea of the ram moving on te Navmesh, instead we want to give it it's own movement
        //Setting the NavMesh destination
        navMesh.destination = newPos;

        //If we're within a radius of our desired point, find a new point to seek to
        if (ram.transform.position.x >= newPos.x - 1 && ram.transform.position.x <= newPos.x + 1 &&
           ram.transform.position.z >= newPos.z - 1 && ram.transform.position.z <= newPos.z + 1)
        {
            timer += Time.deltaTime;

            if (timer >= idleTimer)
            {
                newPos = ram.GetComponent<Ram>().GetNewDestination();
                timer = 0;
            }
        }

        //************************************************************************************
        //      LITTLE PIECE OF WANDER CODE TO USE, JUST GOTTA IMPLEMENT IT PROPERLY
        //************************************************************************************
        //public static Vector3 RandomNavSphere(Vector3 origin, float distance, int layermask)
        //{
        //    Vector3 randomDirection = UnityEngine.Random.insideUnitSphere * distance;

        //    randomDirection += origin;

        //    NavMeshHit navHit;

        //    NavMesh.SamplePosition(randomDirection, out navHit, distance, layermask);

        //    return navHit.position;
        //}
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //We need to set this to false as we leave this state, otherwise when we come into the Charge state, 
        // "isWandering" will still be leftover as true, and the Ram will immediately leave the Charge state back into wander.
        ram.GetComponent<Animator>().SetBool("isWandering", false);
        navMesh.enabled = false;
    }
}
