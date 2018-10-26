using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUSeekSheep : StateMachineBehaviour
{
    GameObject CPU;
    CPU CPUScript;

    Vector3[] currentPath = null;
    float pathFindTimer = 0.0f;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CPU = animator.gameObject;
        CPUScript = CPU.GetComponent<CPU>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // state change
        if (CPUScript.HeldSheep)
            animator.SetBool("isSheepHeld", true);

        // increment timer
        pathFindTimer += Time.deltaTime;

        // wait one second before finding a path
        if (pathFindTimer > 0.5f)
        {
            currentPath = PathManager.Instance.FindPath(CPU, CPUScript.FindNearestSheep());
            pathFindTimer = 0.0f;
        }

        // if there's a path
        if (currentPath.Length > 0)
        {
            Vector3 ray = currentPath[1] - currentPath[0];
            ray.Normalize();
            CPUScript.Move(ray.x, ray.z); // move along path
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
    }
}
