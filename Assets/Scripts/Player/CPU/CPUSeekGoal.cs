using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUSeekGoal : StateMachineBehaviour
{
    // CPU object and script
    private GameObject CPU;
    private CPU CPUScript;

    // AI customization variables
    private float artificialThinkTime;

    // pathfinding relevant variables
    private Vector3[] currentPath = null;
    private float pathFindTimer = 0.0f;
    private bool isHoldingBlackSheep;

    /// <summary>
    /// cache the CPU object and script. Determine what type of sheep is being held
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        CPU = animator.gameObject;
        CPUScript = CPU.GetComponent<CPU>();
        
        // get artificial think time
        artificialThinkTime = CPUScript.ArtificialThinkTime;

        // if the sheep worth is negative
        if (CPUScript.HeldSheep.GetComponent<Sheep>().score < 0)
            isHoldingBlackSheep = true; // black sheep is held
        else
            isHoldingBlackSheep = false; // normal sheep held
    }

    /// <summary>
    /// find path to relevant goal every couple of seconds or so, then move along that path
    /// </summary>
    /// <param name="animator"></param>
    /// <param name="stateInfo"></param>
    /// <param name="layerIndex"></param>
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        // "think" for an arbritrary amount of time before performing actions
        if (artificialThinkTime > 0.0f)
        {
            artificialThinkTime -= Time.deltaTime;
            return;
        }

        // state change
        if (!CPUScript.HeldSheep)
            animator.SetBool("isSheepHeld", false);

        // increment timer
        pathFindTimer += Time.deltaTime;

        // wait a bit before finding a path
        if (pathFindTimer > 0.5f)
        {
            // if holding black sheep, locate the goals of the highest scoring player
            if (isHoldingBlackSheep)
                currentPath = PathManager.Instance.FindPath(CPU, PathManager.Instance.FindWinningGoal(true, CPU));
            else
                currentPath = PathManager.Instance.FindPath(CPU, PathManager.Instance.FindOwnGoal(CPU));

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
}
