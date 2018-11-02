using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RamCharge : StateMachineBehaviour {

    Transform player;
    GameObject ram;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Initiliasing our player for easier access throughout this script
        player = animator.GetComponent<Ram>().player.transform;
        ram = animator.gameObject;

        //We don't want the Ram moving with the NavMesh during this charge, as we don't want him avoiding other objects. 
        // He'll simply be charging in a straight line and a navMeshAgent will get in the way of that
        animator.GetComponent<NavMeshAgent>().enabled = false;
        animator.transform.LookAt(player);
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Vector3 newPos;
        Vector3 newDir;

        //Keep looking at the player. Intimidate them
        ram.transform.LookAt(player);

        //(Destination - Current)
        newPos = (player.position - ram.transform.position);
        newDir = newPos.normalized;

        //Drawing the path
        Debug.DrawLine(ram.transform.position, newPos);

        //Move towards that player
        ram.transform.position += newDir * 5;

    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
