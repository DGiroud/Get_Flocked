using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class PathManager : MonoBehaviour
{
    // singleton instance
    #region singleton

    private static PathManager instance;

    /// <summary>
    /// getting for singleton instance of PathManager
    /// </summary>
    public static PathManager Instance
    {
        get
        {
            return instance;
        }
    }

    #endregion

    /// <summary>
    /// initialises singleton instance
    /// </summary>
    private void Start()
    {
        instance = this;
    }

    /// <summary>
    /// returns an array of Vector3s by using navmeshes to find a
    /// path between source and destination
    /// </summary>
    /// <param name="source">the agent which you want to start the path at (needs navmesh)</param>
    /// <param name="destination">the location you want to find a path to</param>
    /// <returns>an array of points which act as corners on a path</returns>
    public Vector3[] FindPath(GameObject source, GameObject destination)
    {
        // get the NavMeshAgent off of the source object
        NavMeshAgent navMeshAgent = source.GetComponent<NavMeshAgent>();
        NavMeshPath navMeshPath = new NavMeshPath();

        // if there is no NavMeshAgent
        if (!navMeshAgent)
            navMeshAgent = source.AddComponent<NavMeshAgent>(); // then add it

        navMeshAgent.enabled = true; // enable navMeshAgent
        navMeshAgent.CalculatePath(destination.transform.position, navMeshPath); // calculate navMeshPath
        navMeshAgent.enabled = false; // disable navMeshAgent

        // return the path
        return navMeshPath.corners;
    }

    /// <summary>
    /// returns an array of Vector3s by using navmeshes to find a
    /// path between source and destination
    /// </summary>
    /// <param name="source">the agent which you want to start the path at (needs navmesh)</param>
    /// <param name="destination">the location you want to find a path to</param>
    /// <returns>an array of points which act as corners on a path</returns>
    public Vector3[] FindPath(GameObject source, Transform destination)
    {
        // get the NavMeshAgent off of the source object
        NavMeshAgent navMeshAgent = source.GetComponent<NavMeshAgent>();
        NavMeshPath navMeshPath = new NavMeshPath();

        // if there is no NavMeshAgent
        if (!navMeshAgent)
            navMeshAgent = source.AddComponent<NavMeshAgent>(); // then add it

        navMeshAgent.enabled = true; // enable navMeshAgent
        navMeshAgent.CalculatePath(destination.position, navMeshPath);
        navMeshAgent.enabled = false; // disable navMeshAgent

        // return the path
        return navMeshPath.corners;
    }

    /// <summary>
    /// returns an array of Vector3s by using navmeshes to find a
    /// path between source and destination
    /// </summary>
    /// <param name="source">the agent which you want to start the path at (needs navmesh)</param>
    /// <param name="destination">the location you want to find a path to</param>
    /// <returns>an array of points which act as corners on a path</returns>
    public Vector3[] FindPath(GameObject source, Vector3 destination)
    {
        // get the NavMeshAgent off of the source object
        NavMeshAgent navMeshAgent = source.GetComponent<NavMeshAgent>();
        NavMeshPath navMeshPath = new NavMeshPath();

        // if there is no NavMeshAgent
        if (!navMeshAgent)
            navMeshAgent = source.AddComponent<NavMeshAgent>(); // then add it

        navMeshAgent.enabled = true; // enable navMeshAgent
        navMeshAgent.CalculatePath(destination, navMeshPath);
        navMeshAgent.enabled = false; // disable navMeshAgent

        // return the path
        return navMeshPath.corners;
    }

    /// <summary>
    /// debug function which draws the path between two objects (in scene view)
    /// </summary>
    /// <param name="path">the path to draw</param>
    public void DrawPath(Vector3[] path)
    {
        for (int i = 0; i < path.Length - 1; i++)
        {
            Debug.DrawLine(path[i], path[i + 1]);
        }
    }
}
