using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    [SerializeField]
    private int goalID;

    /// <summary>
    /// handles the collision of a sheep with the goal
    /// </summary>
    /// <param name="other">the object which passed through the goals</param>
    private void OnTriggerEnter(Collider other)
    {
        // if the collided object is a sheep
        if (other.transform.parent.CompareTag("Sheep"))
        {
            int sheepWorth = other.gameObject.transform.parent.GetComponent<Sheep>().CurrentTier.score;

            // destroy sheep
            SheepManager.Instance.DestroySheep(other.gameObject);
            ScoreManager.Instance.AddScore(goalID, sheepWorth);
        }
    }
}
