using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    public int goalID;

    [SerializeField]
    private ParticleSystem fireWorksTrail;
    [SerializeField]
    private ParticleSystem fireWorksWhiteNoise;

    /// <summary>
    /// handles the collision of a sheep with the goal
    /// </summary>
    /// <param name="other">the object which passed through the goals</param>
    private void OnTriggerEnter(Collider other)
    {
        // if the collided object is a sheep
        if (other.CompareTag("Sheep"))
        {
            BaseActor player = other.GetComponentInParent<BaseActor>();
            int sheepWorth = other.GetComponent<Sheep>().score;

            // destroy sheep
            SheepManager.Instance.DestroySheep(other.gameObject);
            ScoreManager.Instance.AddScore(goalID, sheepWorth);
            ScoreManager.Instance.IncrementGoalCount(player.actorID);

            fireWorksTrail.Play();
            fireWorksWhiteNoise.Play();
        }
    }
}
