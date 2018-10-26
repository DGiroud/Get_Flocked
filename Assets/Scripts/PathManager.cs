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
    
    public Transform FindValuableSheep()
    {
        // get all currently spawned sheep
        GameObject[] activeSheep = SheepManager.Instance.sheepPool.GetActiveSheep();

        // initialise helper variables
        Transform output = null;
        float highestWorth = Mathf.NegativeInfinity;

        // iterate over all active sheep
        for (int i = 0; i < activeSheep.Length; i++)
        {
            GameObject sheep = activeSheep[i]; // get sheep
            Sheep sheepScript = sheep.GetComponent<Sheep>();

            // ignore sheep if it's being held already
            if (sheep.transform.parent)
                continue;

            // get distance between this sheep and CPU
            float currentWorth = sheepScript.score;

            // if new minimum distance, update distance
            if (currentWorth > highestWorth)
            {
                highestWorth = currentWorth;
                output = sheep.transform;
            }
        }
        return null;
    }
    
    /// <summary>
    /// helper function which scans all the spawned sheep, and determines
    /// which one is the closest
    /// </summary>
    /// <returns>the position of the nearest sheep</returns>
    public Transform FindNearestSheep(GameObject source)
    {
        // get all currently spawned sheep
        GameObject[] activeSheep = SheepManager.Instance.sheepPool.GetActiveSheep();

        // initialise helper variables
        Transform output = null;
        float minDist = Mathf.Infinity;

        // iterate over all active sheep
        for (int i = 0; i < activeSheep.Length; i++)
        {
            GameObject sheep = activeSheep[i]; // get sheep

            // ignore sheep if it's being held already
            if (sheep.transform.parent)
                continue;

            // get distance between this sheep and CPU
            float dist = Vector3.Distance(source.transform.position, sheep.transform.position);

            // if new minimum distance, update distance
            if (dist < minDist)
            {
                minDist = dist;
                output = sheep.transform;
            }
        }

        return output;
    }

    /// <summary>
    /// helper function which grabs all the goals in the scene and returns
    /// the CPU's own goal
    /// </summary>
    /// <returns>the transform of the goal which belongs to this particular CPU</returns>
    public Transform FindOwnGoal(GameObject source)
    {
        // get all the goals
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");

        // iterate over said goals
        for (int i = 0; i < goals.Length; i++)
        {
            // get the goal script of each goal
            Goal goalScript = goals[i].GetComponentInChildren<Goal>();

            // if the goal belongs to this actor
            if (goalScript.goalID == source.GetComponent<BaseActor>().actorID)
            {
                return goalScript.transform; // return the position
            }
        }

        // couldn't find own goal (this shouldn't ever happen)
        return null;
    }

    /// <summary>
    /// helper function which grabs all the goals in the scene and returns
    /// the closest possible goal
    /// </summary>
    /// <returns>the transform of the closest goal</returns>
    public Transform FindNearestGoal(GameObject source)
    {
        // get all the goals
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");

        // initialise helper variables
        Transform output = null;
        float minDist = Mathf.Infinity;

        // iterate over all active sheep
        for (int i = 0; i < goals.Length; i++)
        {
            // get the goal script of each goal
            GameObject goal = goals[i];

            // get distance between this sheep and CPU
            float dist = Vector3.Distance(transform.position, goal.transform.position);

            // if new minimum distance, update distance
            if (dist < minDist)
            {
                minDist = dist;
                output = goal.transform;
            }
        }

        // couldn't find nearest goal (this shouldn't ever happen)
        return null;
    }

    /// <summary>
    /// helper function which grabs all the goals in the scene and returns
    /// the winning opponent's goal
    /// </summary>
    /// <returns>the transform of the goal which belongs to the current front-runner</returns>
    public Transform FindWinningGoal(bool ignoreSelf = false, GameObject actor = null)
    {
        // get all the goals
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        
        // get the ID of the highest scoring player
        int highestScoringPlayer = ScoreManager.Instance.GetHighestScoringPlayer(ignoreSelf, actor);

        // iterate over goals
        for (int i = 0; i < goals.Length; i++)
        {
            // get the goal script of each goal
            Goal goalScript = goals[i].GetComponentInChildren<Goal>();

            // if the goal belongs to the current front-runner
            if (goalScript.goalID == highestScoringPlayer)
                return goalScript.transform; // return the position
        }

        // couldn't find opponents goal (this shouldn't ever happen)
        return null;
    }
}
