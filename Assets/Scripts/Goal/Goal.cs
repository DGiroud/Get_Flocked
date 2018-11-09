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
            // get the player that's parenting the colliding sheep
            BaseActor player = other.GetComponentInParent<BaseActor>();

            // if the colliding sheep has no parent (e.g. it's scoring itself) then...
            if (player != null)
                ScoreManager.Instance.IncrementGoalCount(player.actorID); // increment goal count

            // get how much points the sheep is worth
            int sheepWorth = other.GetComponent<Sheep>().score;

            // destroy sheep & add score
            SheepManager.Instance.DestroySheep(other.gameObject);
            ScoreManager.Instance.AddScore(goalID, sheepWorth);
            ScoreManager.Instance.IncrementTotalSheep();

            fireWorksTrail.Play();
            fireWorksWhiteNoise.Play();
        }
    }
}
