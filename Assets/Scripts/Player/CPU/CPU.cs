using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CPU : BaseActor
{
    [HideInInspector]
    public Transform destination;

    private NavMeshAgent navMeshAgent;
    public NavMeshPath currentPath;

    /// <summary>
    /// 
    /// </summary>
    void Start ()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        navMeshAgent.enabled = false;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform FindNearestSheep()
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
            float dist = Vector3.Distance(transform.position, sheep.transform.position);

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
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform FindOwnGoal()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");

        for (int i = 0; i < goals.Length; i++)
        {
            Goal goalScript = goals[i].GetComponentInChildren<Goal>();

            if (goalScript.goalID == actorID)
            {
                return goalScript.transform;
            }
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public Transform FindOpponentsGoal()
    {
        GameObject[] goals = GameObject.FindGameObjectsWithTag("Goal");
        int highestScoringPlayer = GetHighestScoringPlayer();

        for (int i = 0; i < goals.Length; i++)
        {
            Goal goalScript = goals[i].GetComponentInChildren<Goal>();

            if (goalScript.goalID == highestScoringPlayer)
                return goalScript.transform;
        }

        return null;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetHighestScoringPlayer()
    {
        PlayerScores[][] playerScores = ScoreManager.GetPlayerScores();
        float highestScore = Mathf.NegativeInfinity;
        int relevantPlayerID = Random.Range(0, 3);

        for (int i = 0; i < ScoreManager.GetPlayerScores().Length; i++)
        {
            if (i == actorID)
                continue;

            PlayerScores[] currentPlayerScores = playerScores[i];
            int currentPlayerScore = currentPlayerScores[LevelManager.GetCurrentRound()].score;

            if (currentPlayerScore > highestScore)
            {
                highestScore = currentPlayerScore;
                relevantPlayerID = i;
            }
        }

        return relevantPlayerID;
    }
}
