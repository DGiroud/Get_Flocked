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
    //[SerializeField]
    //private ParticleSystem[] scoreParticles;

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
            
            // get sheep's script for ease of access
            Sheep sheepScript = other.GetComponent<Sheep>();

            // if this is a golden sheep, increment gold sheep count (for this goal)
            if (sheepScript.IsGoldSheep)
                ScoreManager.Instance.IncrementGoldCount(goalID);

            // destroy sheep & add score
            SheepManager.Instance.DestroySheep(other.gameObject);
            ScoreManager.Instance.AddScore(goalID, sheepScript.score);
            ScoreManager.Instance.IncrementTotalSheep();

            fireWorksTrail.Play();
            fireWorksWhiteNoise.Play();

            //switch (sheepScript.score)
            //{
            //    case -15:
            //        scoreParticles[0].Play();
            //        break;
            //    case 10:
            //        scoreParticles[1].Play();
            //        break;
            //    case 20:
            //        scoreParticles[2].Play();
            //        break;
            //    case 30:
            //        scoreParticles[3].Play();
            //        break;
            //}
        }
    }
}
