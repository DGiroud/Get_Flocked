using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : BaseActor
{
    Vector3 translation;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	/// <summary>
    /// handle player input
    /// </summary>
	override public void Update ()
    {
        // movement input
        Movement();

        // action input
        Kick();

        // call update on BaseActor
        base.Update();
    }

    /// <summary>
    /// placeholder movement function for player. Handles the translation and
    /// rotation of the player in the x and z axis
    /// </summary>
    public void Movement()
    {
        // x & z translation mapped to horizontal & vertical respectively
        if (Input.GetAxisRaw("Horizontal") == 0.0f && Input.GetAxisRaw("Vertical") == 0.0f)
            return;
        
        translation.x = Input.GetAxisRaw("Horizontal");
        translation.z = Input.GetAxisRaw("Vertical");

        // rotation handling
        transform.rotation = Quaternion.LookRotation(translation);

        // multiply by speed and delta time
        translation *= speed * Time.deltaTime;

        // perform movement
        transform.Translate(translation, Space.World);
    }

    public void Kick()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            ReleaseSheep();
        }
    }
}
