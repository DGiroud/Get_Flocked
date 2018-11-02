using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamStepback : StateMachineBehaviour {

    //This state, whilst small, is responsible for the visual stepback or "kicking feet" that the Ram will do before charging a sighted player
    // In this state, the Ram will continually look towards the player, and slowly move backwards as a "wind-up", before entering the Charge.

    float chargeDelay;
    float timer;
    GameObject player;
    Ram ram;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Set animator for charge preperation here
        player = animator.GetComponent<Ram>().player;
        ram = animator.GetComponent<Ram>();

        chargeDelay = animator.GetComponent<Ram>().chargeDelay;
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Update animation?
        timer += Time.deltaTime;

        //We want to keep the Ram looking at the player while we prepare to charge
        //************************************************************************
        //                      PLAYER IS RETURNING NULL
        //************************************************************************
        animator.transform.LookAt(ram.player.transform);

        if(timer >= chargeDelay)
        {
            animator.SetBool("isCharging", true);
        }
    }

    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
