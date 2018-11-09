using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum RotateMode
{
    Constant,
    Periodic,
    ConstantRandom
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
    public int rotateSpeed; // how fast go

    // periodic rotation relevent variables
    [Header("Periodic Rotation")]
    [Tooltip("the angle in degrees that each periodic rotation rotates")]
    public float rotationAngle;
    [Tooltip("the range of times with which the rotation may start")]
    public Vector2 rotateTime; // range of rotation times
    private float rotateTimer; // timer used to determine when to rotate

    public float min;
    public float max;

    bool isRotatingClockwise;
  



    /// <summary>
    /// initialise rotateTimer time to a random float in given range
    /// </summary>
	void Awake ()
    {
        rotateTimer = Random.Range(min, max);
        isRotatingClockwise = false;
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
                rotateTimer -= Time.deltaTime; // decrement timer

                if (rotateTimer < 0.0f)
                {
                    rotateTimer = Random.Range(min, max);
                }
                Debug.Log(rotateTimer);
                // if timer runs out
                if (rotateTimer < 7.5f)
                {
                    rotateTimer -= Time.deltaTime; // decrement timer
                    // do rotation
                    //StopAllCoroutines();
                    isRotatingClockwise = false;
                    // reset timer

                }
                else
                {
                    rotateTimer -= Time.deltaTime; // decrement timer
                    // do rotation
                    //StopAllCoroutines();
                    isRotatingClockwise = true;
                    // reset timer
                }
                StartCoroutine(Rotate(isRotatingClockwise));
                break;

                //starts off as constant then transitions into periodic
                //then back to constant rotation

           case RotateMode.ConstantRandom:
                //decrement timer
                rotateTimer -= Time.deltaTime;

                transform.Rotate(0, rotateSpeed * Time.deltaTime, 0);
                //if the timer runs out
                if (rotateTimer <= 0.0f)
                {
                    //do rotation
                    StopAllCoroutines();
                    StartCoroutine(Rotate());

                    //reset timer
                    rotateTimer = Random.Range(rotateTime.x, rotateTime.y);

                }

                break;



        }
       

    }

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
        if (transform.rotation != desiredRotation)
        {
            // perform rotation
            transform.rotation = Quaternion.Lerp(transform.rotation, desiredRotation, Time.deltaTime * rotateSpeed);
        }
        //rotateTimer = Random.Range(min, max);
        //else
        //{
        //    rotateTimer = Random.Range(min, max);
        //    yield return null;
        //}


    }

}
