using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateMode
{
    Constant,
    Periodic,
    PeriodicV2,
    PeriodicRandom
}

public class Rotator : MonoBehaviour
{
    // rotate mode, either constant or periodic
    [Header("Rotate Mode")]
    [Tooltip("the mode of rotation. \nConstant: for consistent rotation at all times" +
        "\nPeriodic: for rotating an arbitrary amount after an arbitrary amount of time")]
    public RotateMode rotateMode = RotateMode.Periodic;


    [Header("Rotate Speed")]
    [Tooltip("the speed that the object rotates")]
    public float rotateSpeed; // how fast go

    // periodic rotation relevent variables
    [Header("Periodic Rotation")]
    [Tooltip("the angle in degrees that each periodic rotation rotates")]
    public float rotationAngle;
    [Tooltip("the range of times with which the rotation may start")]
    public Vector2 rotateTime; // range of rotation times
    private float rotateTimer; // timer used to determine when to rotate
    private float rotateToggleTimer; //toggles between timer

    public float min;
    public float max;

    bool isRotatingClockwise;
    private bool hasStoppedRotation = false;


    /// <summary>
    /// initialise rotateTimer time to a random float in given range
    /// </summary>
	void Awake ()
    {
        rotateTimer = Random.Range(min, max);
        rotateToggleTimer = Random.Range(min, max);
        isRotatingClockwise = false;
        hasStoppedRotation = true;

    }
	
    /// <summary>
    /// calls the relevant rotation functions depending on the given rotate mode
    /// </summary>
	void Update ()
    {
        switch (rotateMode)
        {
            // constant rotation, easy
            case RotateMode.Constant:
                transform.Rotate(0, rotateSpeed * Time.deltaTime, 0); 
                break;

            // periodic rotation, not as easy
            case RotateMode.Periodic:
                if (rotateTimer < 1.0f)
                {
                    rotateTimer = Random.Range(min, max);
                }

                // if timer runs out
                if (rotateTimer < 1.5f)
                {
                    hasStoppedRotation = false;
                }
                else
                {
                    hasStoppedRotation = true;
                }
                rotateTimer -= Time.deltaTime; // decrement timer
                StartCoroutine(NoRotate(hasStoppedRotation));
                break;

                //constant/periodic
            case RotateMode.PeriodicV2:
                if (rotateTimer < 0.0f)
                {
                    rotateTimer = Random.Range(min, max);
                }
                // if timer runs out
                if (rotateTimer < 7.5f)
                {
                    isRotatingClockwise = false;
                }
                else
                {
                    isRotatingClockwise = true;
                }
                rotateTimer -= Time.deltaTime; //decrement timer
                StartCoroutine(Rotate(isRotatingClockwise));
                break;

            case RotateMode.PeriodicRandom:

                rotateTimer -= Time.deltaTime; //decrement timer

                // if timer runs out
                if (rotateTimer < 0.0f)
                {
                    isRotatingClockwise = !isRotatingClockwise;
                    StopAllCoroutines();
                    StartCoroutine(Rotate(isRotatingClockwise));

                    rotateTimer = Random.Range(min, max);
                }

                break;
        }
    }

    #region RotateIEnumerator
    /// <summary>
    /// subroutine which performs a restricted rotation on an object
    /// </summary>
    /// <returns>the coroutine</returns>
    private IEnumerator Rotate(bool counterClockwise = false)
    {
        Quaternion desiredRotation;
        if (counterClockwise == true)
        {
            // get desired rotation quaternion
            desiredRotation = Quaternion.Euler(0, -rotationAngle, 0) * transform.rotation;
        }
        else
        {
            // get desired rotation quaternion
            desiredRotation = Quaternion.Euler(0, rotationAngle, 0) * transform.rotation;
        }
        yield return new WaitForSeconds(5.0f);

        // loop until the desired rotation is reached
        while (transform.rotation != desiredRotation)
        {
            // perform rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
        }
    }
    #endregion
    #region NoRotateIEnumerator
    /// <summary>
    /// subroutine which performs a resricted rotation
    /// </summary>
    /// <param name="isNotMoving"></param>
    /// <returns></returns>
    private IEnumerator NoRotate(bool isNotMoving)
    {
        Quaternion stopRotation;
        if (isNotMoving == true)
        {
            //gets the stop rotation quaternion
            stopRotation = Quaternion.Euler(0, -rotationAngle, 0) * transform.rotation;

        }
        else
        {
            //setting stop rotation to the transformed rotation
            stopRotation = transform.rotation;
        }
        
        //loops untill the stopped rotation is reached
        while (transform.rotation != stopRotation)
        {
            //preforms the rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, stopRotation, Time.deltaTime * rotateSpeed);
        yield return null;
        }
    }
    #endregion
}
