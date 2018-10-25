using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUSeekGoal : StateMachineBehaviour
{
    GameObject CPU;
    CPU CPUScript;

    Vector3[] currentPath = null;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CPU = animator.gameObject;
        CPUScript = CPU.GetComponent<CPU>();
    }

    // OnStateUpdate is called on each Update frame between OnStateEnter and OnStateExit callbacks
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        currentPath = PathManager.Instance.FindPath(CPU, CPUScript.FindOwnGoal());
        PathManager.Instance.DrawPath(currentPath);

        if (!CPUScript.HeldSheep)
            animator.SetBool("isSheepHeld", false);

        if (currentPath.Length > 0)
        {
            Vector3 ray = currentPath[1] - currentPath[0];
            ray.Normalize();
            CPUScript.Move(ray.x, ray.z);
        }
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {

    }
}
