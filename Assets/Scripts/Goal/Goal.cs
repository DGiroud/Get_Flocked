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

    [Header("Sheep Scoring Effects")]
    public GameObject[] scoreParticles;
    private GameObject[] sceneParticles = { null, null, null, null };
    private float[] particleTimer = { 0, 0, 0, 0 };

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

            //Creating the score particle effects when a sheep is scored
            switch (sheepScript.score)
            {
                case -15:
                    sceneParticles[0] = Instantiate(scoreParticles[0], GetComponentInParent<Transform>().position,
                                        new Quaternion(-0.7071068f, 0, 0, 0.7071068f)); //Upright rotation
                    break;
                case 10:
                    sceneParticles[1] = Instantiate(scoreParticles[1], GetComponentInParent<Transform>().position,
                                        new Quaternion(-0.7071068f, 0, 0, 0.7071068f)); //Upright rotation
                    break;
                case 20:
                    sceneParticles[2] = Instantiate(scoreParticles[2], GetComponentInParent<Transform>().position,
                                        new Quaternion(-0.7071068f, 0, 0, 0.7071068f)); //Upright rotation
                    break;
                case 30:
                    sceneParticles[3] = Instantiate(scoreParticles[3], GetComponentInParent<Transform>().position,
                                        new Quaternion(-0.7071068f, 0, 0, 0.7071068f)); //Upright rotation
                    break;
            }
        }
    }
}
