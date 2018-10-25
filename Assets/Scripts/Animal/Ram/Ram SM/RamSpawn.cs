using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSpawn : StateMachineBehaviour {

    Light spotlight;
    float timer = 0.0f;

    Vector3 ramTarget;
    Vector3 ramSpawnLocation;
    bool destination = false;


    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //If higher than the ground, rocket downwards like a meteorite

        //Taking control of the RamSpotlight
        spotlight = RamManager.Instance.GetComponentInChildren<Light>();
        spotlight.enabled = true;
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        //Shrink the size of the ray throughout it's lifetime
        spotlight.spotAngle -= 0.045f;

        //When the spotlight reaches a small enough size, start flashing red to suggest it's imminent danger
        if(spotlight.spotAngle <= 10)
        {
            timer += Time.deltaTime;

            if(timer >= 0.5f)
            {
                FlashRed();
                timer = 0.0f;
            }
        }

        if(spotlight.spotAngle == 1)
        {
            spotlight.color = Color.red;

            //Raycast towards the point where the spotlight, but keep the spheres rotating
            // Save the point of the spawner a few seconds after the fact, draw a line between the two using vector directions,
            //  spawn the ram, turn off gravity, rotate ram to face ground point, and add force

            if (destination == false)
            {
                ramTarget = spotlight.transform.position;
                ramTarget.y = spotlight.transform.position.y - 20;
                destination = true;
            }

            spotlight.transform.LookAt(ramTarget);

        }

        //If we've hit the ground, create a dust storm, change rotation to be standing upright, and switch state to stunned
        if (animator.transform.position.y <= 0.5f)
        {
            //Explosion of dust here
            //Rotation set here
            //Set State here
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }

    //Used within update, makes the spotlight flash red when it's small enough
    void FlashRed()
    {
        if (spotlight.color == Color.red)
            spotlight.color = Color.white;

        else if (spotlight.color == Color.white)
            spotlight.color = Color.red;
    }
}
