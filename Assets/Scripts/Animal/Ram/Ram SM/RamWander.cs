using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    float movementSpeed;

    //Pathfinding variables
    Vector3[] currentPath;
    float pathFindTimer = 0.0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initialising ram for easier access throughout this script
        ram = animator.gameObject;
        //Finding a new position to Wander to
        newPos = ram.GetComponent<Ram>().GetNewDestination();
        movementSpeed = ram.GetComponent<Ram>().wanderSpeed;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentPath = PathManager.Instance.FindPath(ram, newPos);
        ram.transform.LookAt(newPos);

        //move towards new location
        //Work out direction (Destination - current)
        if (currentPath != null && currentPath.Length > 0)
        {
            newPos = (currentPath[1] - currentPath[0]).normalized;

            //Drawing the path
            PathManager.Instance.DrawPath(currentPath);
        }

        ram.transform.position += (newPos);

        if (ram.transform.position.x >= newPos.x - 1 && ram.transform.position.x <= newPos.x + 1 &&
           ram.transform.position.z >= newPos.z - 1 && ram.transform.position.z <= newPos.z + 1)
        {
            newPos = ram.GetComponent<Sheep>().GetNewDestination();
        }


        // * Charge player if one comes within range
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
