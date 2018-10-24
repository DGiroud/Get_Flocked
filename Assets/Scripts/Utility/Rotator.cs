using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateMode
{
    constant,
    periodic,
    constantPeriodicRandom
}

public class Rotator : MonoBehaviour
{
    // rotate mode, either constant or periodic
    [Header("Rotate Mode")]
    [Tooltip("the mode of rotation. \nConstant: for consistent rotation at all times" +
        "\nPeriodic: for rotating an arbitrary amount after an arbitrary amount of time")]
    public RotateMode rotateMode = RotateMode.periodic;

    [Header("Rotate Speed")]
    [Tooltip("the speed that the object rotates")]
    public int rotateSpeed; // how fast go

    // periodic rotation relevent variables
    [Header("Periodic Rotation")]
    [Tooltip("the angle in degrees that each periodic rotation rotates")]
    public float rotationAngle;
    [Tooltip("the range of times with which the rotation may start")]
    public Vector2 rotateTime; // range of rotation times
    private float rotateTimer; // timer used to determine when to rotate
    private float randomRotateRotation;


    /// <summary>
    /// initialise rotateTimer time to a random float in given range
    /// </summary>
	void Awake ()
    {
        rotateTimer = Random.Range(rotateTime.x, rotateTime.y);
    }
	
    /// <summary>
    /// calls the relevant rotation functions depending on the given rotate mode
    /// </summary>
	void Update ()
    {
        switch (rotateMode)
        {
            // constant rotation, easy
            case RotateMode.constant:
                transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); 
                break;

            // periodic rotation, not as easy
            case RotateMode.periodic:
                rotateTimer -= Time.deltaTime; // decrement timer

                // if timer runs out
                if (rotateTimer <= 0.0f)
                {
                    // do rotation
                    StopAllCoroutines();
                    StartCoroutine(Rotate());

                    // reset timer
                    rotateTimer = Random.Range(rotateTime.x, rotateTime.y);
                }
                break;

            case RotateMode.constantPeriodicRandom:
                rotateTimer -= Time.deltaTime;

                if(rotateTimer <= 0.0f)
                {
                    StopAllCoroutines();
                    StartCoroutine(RandomRotate());

                    rotateTimer = Random.Range(rotateTime.x, rotateTime.y);
                }

                break;
        }
    }

    /// <summary>
    /// subroutine which performs a restricted rotation on an object
    /// </summary>
    /// <returns>the coroutine</returns>
    private IEnumerator Rotate()
    {
        // get desired rotation quaternion
        Quaternion desiredRotation = Quaternion.Euler(0, rotationAngle, 0) * transform.rotation;

        // loop until the desired rotation is reached
        while (transform.rotation != desiredRotation)
        {
            // perform rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }
    }
    private IEnumerator RandomRotate()
    {



        Quaternion desiredRandomRotation = Quaternion.Euler(new Vector3(0, randomRotateRotation, 0)) * transform.rotation;

        while (desiredRandomRotation != transform.rotation)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRandomRotation, Time.deltaTime * rotateSpeed);
            yield return null;
        }


    }

}
