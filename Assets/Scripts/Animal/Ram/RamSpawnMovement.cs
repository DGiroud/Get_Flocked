using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamSpawnMovement : MonoBehaviour {

    [System.Serializable]
    public struct SphereRotation
    {
        public Vector2 innerSphereRange;
        public Vector2 middleSphereRange;
        public Vector2 outerSphereRange;
    }

    private GameObject sphere;
    private GameObject sineSphere;
    private GameObject innerSphere;
    public SphereRotation sphereRotation;
    Vector3 debugLine;

    //These 3 range floats are what we use for the sphere rotation. Every few seconds the values are randomised
    private float innerRange;
    private float middleRange;
    private float outerRange;
    private float rangeTimer; //Ugh, another timer?

	// Use this for initialization
	void Start () {
        //Initialising all 3 rotating spheres for reference throughout this script
        sphere = GameObject.Find("RamSphere");
        sineSphere = GameObject.Find("SineSphere");

        innerSphere = GameObject.Find("InnerSphere");

        //Finding a random speed for the spheres to rotate with
        NewRange();

        //Begin the sphere at a random rotation around the y axis, to shake it up each game
        sphere.transform.rotation = Quaternion.Euler(0,startingRotation(),0);
	}
	
	// Update is called once per frame
	void Update () {
        rangeTimer += Time.deltaTime;

        sphere.transform.Rotate(0, -innerRange, 0);
        sineSphere.transform.Rotate(0, -middleRange, 0);

        Debug.DrawLine(transform.position, debugLine, Color.red);

        if (rangeTimer >= 5.0f)
            NewRange();
	}

    //Resets the random numbers for rotation speed
    private void NewRange()
    {
        innerRange = Random.Range(-sphereRotation.innerSphereRange.x, -sphereRotation.innerSphereRange.y);
        middleRange = Random.Range(-sphereRotation.middleSphereRange.x, -sphereRotation.middleSphereRange.y);
        outerRange = Random.Range(-sphereRotation.outerSphereRange.x, -sphereRotation.outerSphereRange.y);
    }

    private float startingRotation()
    {
        return Random.Range(0, 360);
    }
}
