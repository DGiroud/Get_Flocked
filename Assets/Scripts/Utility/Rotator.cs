using System.Collections;
using UnityEngine;

public enum RotateMode
{
    Constant,
    Periodic,
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
    private float rotateTimer; // timer used to determine when to rotate
    private int rotateCounter; //keeps track of how many times it has rotated
    private int rotateToggle; //toggles between timer

    [Tooltip("min range for turn values(INPUT VALUE OTHER THAN 0)")]
    public float minTurnValue;                  //min range
    [Tooltip("max range for turn values(INPUT VALUE OTHER THAN 0)")]
    public float maxTurnValue;                  //max range
    [Tooltip("The range of min value for how many turns can happen")]
    public int minNumberOfTurns = 1;            //min range of turns
    [Tooltip("The range of max value for how many turns can happen")]
    public int maxNumberOfTurns = 5;            //max range of turns

    bool isRotatingClockwise;   //is it rotating clockwise? (true/false)
    private bool hasStoppedRotation = false;    //has it stopped rotating? (this is always set to false)



    /// <summary>
    /// initialise rotateTimer time to a random float in given range
    /// </summary>
    void Awake()
    {
        //picks a min and max turn value for the rotate timer
        rotateTimer = Random.Range(minTurnValue, maxTurnValue);
        //picks a min and max number of turns for rotate toggle
        rotateToggle = Random.Range(minNumberOfTurns, maxNumberOfTurns);

        //assigning bool variables to true or false
        isRotatingClockwise = false;
        hasStoppedRotation = true;

    }

    /// <summary>
    /// calls the relevant rotation functions depending on the given rotate mode
    /// </summary>
    void Update()
    {
        switch (rotateMode)
        {
            // constant rotation, easy
            case RotateMode.Constant:
                transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                break;
                //periodic rotation
            case RotateMode.Periodic:
                rotateTimer -= Time.deltaTime; // decrement timer

                // if timer runs out
                if (rotateTimer < 0.0f)
                {
                    //picks a random number between set values for min and max
                    rotateTimer = Random.Range(minTurnValue, maxTurnValue);
                    //starting the "Rotate" coroutine with paramerters if it has
                    //stopped rotation or not
                    StartCoroutine(Rotate(hasStoppedRotation));
                    //stop the "Rotate" coroutine then starts it again
                    StopAllCoroutines();
                    StartCoroutine(Rotate());
                }

                break;
            // periodic rotation, not as easy
            case RotateMode.PeriodicRandom:
                rotateTimer -= Time.deltaTime; // decrement timer

                
                if (rotateCounter > rotateToggle)
                {
                    //checks to see if the rotation as stopped or not
                    hasStoppedRotation = !hasStoppedRotation;
                    //resets the rotate counter back to 0
                    rotateCounter = 0;
                    //picks a random number between set values for min and max
                    rotateToggle = Random.Range(minNumberOfTurns, maxNumberOfTurns);
                }
                // if the timer runs out
                if (rotateTimer < 0.0f)
                {
                    //picks a random number between min and max turn values
                    rotateTimer = Random.Range(minTurnValue, maxTurnValue);
                    //starts the "Rotate" coroutine with paramaerters to see if it has
                    //stopped rotation or not
                    StartCoroutine(Rotate(hasStoppedRotation));
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
        rotateCounter++;
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

        // loop until the desired rotation is reached
        while (transform.rotation != desiredRotation)
        {
            // perform rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
            yield return null;
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
