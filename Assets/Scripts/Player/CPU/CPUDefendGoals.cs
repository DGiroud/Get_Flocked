using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUDefendGoals : StateMachineBehaviour
{
    private GameObject CPU;
    private CPU CPUScript;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CPU = animator.gameObject;
        CPUScript = CPU.GetComponent<CPU>();

    }
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // state change
        if (CPUScript.HeldSheep)
            animator.SetBool("isSheepHeld", true);


    }
}
