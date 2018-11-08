using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CPUSeekSheep : StateMachineBehaviour
{
    // CPU object and script
    private GameObject CPU;
    private CPU CPUScript;

    // AI customization variables
    private float artificialThinkTime;
    
    // pathfinding relevant variables
    private Vector3[] currentPath = null;
    private float pathFindTimer = 0.0f;

    /// <summary>
    /// cache the CPU object and script
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
    }

    /// <summary>
    /// find path to nearest sheep every couple of seconds or so, then move along that path
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
        if (CPUScript.HeldSheep)
            animator.SetBool("isSheepHeld", true);

        // increment timer
        pathFindTimer += Time.deltaTime;
        GameObject targetSheep = null;

        switch (CPUScript.cpuSeekMode)
        {
            case CPUSeekMode.Nearest:
                targetSheep = SheepManager.Instance.FindNearestSheep(CPU);
                break;
            case CPUSeekMode.HighestWorth:
                targetSheep = SheepManager.Instance.FindMostValuableSheep();
                break;
            case CPUSeekMode.LowestWorth:
                targetSheep = SheepManager.Instance.FindLeastValuableSheep();
                break;
        }

        // wait a bit before finding a path
        if (targetSheep != null && pathFindTimer > 0.5f)
        {
            currentPath = PathManager.Instance.FindPath(CPU, targetSheep, (1 << 3));
            pathFindTimer = 0.0f;
        }
        
        // if there's a path
        if (currentPath != null && currentPath.Length > 0)
        {
            Vector3 ray = currentPath[1] - currentPath[0];
            ray.Normalize();
            CPUScript.Move(ray.x, ray.z); // move along path
        }
    }
}
