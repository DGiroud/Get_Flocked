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
	
	// Update is called once per frame
	override public void Update ()
    {
        Movement();
        base.Update();
    }

    /// <summary>
    /// 
    /// </summary>
    public void Movement()
    {
        // x & z translation mapped to horizontal & vertical respectively
        if (Input.GetAxisRaw("Horizontal") != 0.0f || Input.GetAxisRaw("Vertical") != 0.0f)
        {
            translation.x = Input.GetAxisRaw("Horizontal");
            translation.z = Input.GetAxisRaw("Vertical");

            // rotation handling
            transform.rotation = Quaternion.LookRotation(translation);

            // multiply by speed and delta time
            translation *= speed * Time.deltaTime;

            // perform movement
            transform.Translate(translation, Space.World);
        }

    }
}
