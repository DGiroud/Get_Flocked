using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour
{
    // reference to the camera on this
    private Camera dynamicCamera;

    // reference to all the players in the scene
    private List<GameObject> players;

    // min & max zoom which limits the camera zoom
    [Range(10, 50)]
    public float minZoom;
    [Range(50, 179)]
    public float maxZoom;

    // min and max positions which is used to determine camera
    // position and zoom
    private float xMin, xMax;
    private float zMin, zMax;
    private float greatestDistance;

    /// <summary>
    /// initialise camera and player references
    /// </summary>
	void Start ()
    {
        // camera
        dynamicCamera = GetComponent<Camera>();

        // players
        players = PlayerManager.Instance.players;
	}
	
    /// <summary>
    /// LateUpdate used such that the camera updates one frame after
    /// the player/other objects update
    /// </summary>
	void LateUpdate ()
    {
        // move camera on x and z axes
        Move();

        // zoom camera by adjusting field of view
        Zoom();
	}

    /// <summary>
    /// 
    /// </summary>
    private void Move()
    {
        // set minimum and maximum
        xMin = zMin = Mathf.Infinity;
        xMax = zMax = Mathf.NegativeInfinity;

        // iterate over all players
        for (int i = 0; i < players.Count; i++)
        {
            // get player
            GameObject player = players[i];

            // update minimum and maximum values as required
            if (player.transform.position.x < xMin)
                xMin = player.transform.position.x;

            if (player.transform.position.x > xMax)
                xMax = player.transform.position.x;

            if (player.transform.position.z < zMin)
                zMin = player.transform.position.z;

            if (player.transform.position.z > zMax)
                zMax = player.transform.position.z;
        }

        // find the midpoint between each range
        float xPos = (xMin + xMax) * 0.5f;
        float zPos = (zMin + zMax) * 0.5f;

        // update camera position
        dynamicCamera.transform.position = new Vector3(xPos, dynamicCamera.transform.position.y, zPos - 7);
    }

    /// <summary>
    /// 
    /// </summary>
    private void Zoom()
    {
        // get greatest distance
        greatestDistance = xMax - xMin;

        // create new field of view floating point value
        float newFieldOfView = greatestDistance * 2;

        // check if the new field of view is outside of range
        if (newFieldOfView > maxZoom)
            return;

        if (newFieldOfView < minZoom)
            return;

        dynamicCamera.fieldOfView = newFieldOfView;
    }
}
