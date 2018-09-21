using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sheep : BaseAnimal {

    [Header("Sheep Tier Prefabs")]
    public GameObject smallSheep;
    public GameObject mediumSheep;
    public GameObject largeSheep;

    [Header("Sheep Modifiers")]
    [Range(0, 500)]
    [Tooltip("The Sheep's base movement speed")]
    public float speed;

    [Range(0, 500)]
    [Tooltip("The weight of the sheep, affects the speed at which it can be pushed around by the player")]
    public float mass;

    [Tooltip("The duration that the sheep will remain idle before continuing it's wander movement")]
    public float idleDuration;
    [Tooltip("The duration that the sheep will wander around before continuing it's idle animation")]
    public float wanderDuration;

    //Multipliers for patterned sheep
    [Tooltip("The varying score multipliers to accompany the different patterned sheep")]
    public float[] scoreMultiplier = new float[4] {1.0f, 1.5f, 2.0f, 3.0f};
    [Tooltip("Added possibility for different tiered sheep to have different growth times")]
    public float[] growthTime;

    //Determines the size of the sheep
    int sizeTier;

    // Use this for initialization
    void Start ()
    {
        
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Testing of Grow function
		if(Input.GetKeyDown(KeyCode.E))
        {
            Grow();
        }
	}

    //Whilst the sheep is undisturbed by a player, it will wander around consuming grass. When it does so, it periodically grows in size
    private void WanderBehaviour()
    {

    }

    //Function to upgrade the sheep to the next tier
    private void Grow()
    {
               
    }
}
