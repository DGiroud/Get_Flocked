using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class DynamicCamera : MonoBehaviour
{
    /// <summary>
    /// helper class which stores two vectors, representing co-ordinates
    /// for a bounding box
    /// </summary>
    private class XZBounds
    {
        public Vector2 min, max;

        public XZBounds(float xMin, float xMax, float zMin, float zMax)
        {
            min.x = xMin;
            min.y = zMin; // set y to z value since vector2
            max.x = xMax;
            max.y = zMax; // same with this
        }
    }

    // reference to the camera
    private Camera dynamicCamera;

    // reference to all the players in the scene
    private List<GameObject> players;

    // this offsets the camera's position (for designer freedom)
    [SerializeField]
    [Tooltip("Offsets the camera's position")]
    private Vector3 offset;

    // camera zoom settings
    [Header("Zoom Settings")]
    [Range(10, 50)]
    [Tooltip("The smaller the value; the closer the camera will zoom in when players are close")]
    public float minFieldOfView = 40.0f;
    [Range(50, 179)]
    [Tooltip("The higher the value; the further the camera will zoom out when players are distant")]
    public float maxFieldOfView = 60.0f;
    public float zoomLimiter = 50.0f;

    // velocity value used for smoothdamp buffer
    private Vector3 velocity;

    /// <summary>
    /// initialise camera and player references
    /// </summary>
	void Start ()
    {
        // get camera
        dynamicCamera = GetComponent<Camera>();

        // get players
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
    /// gets the center point of all players and updates camera
    /// position accordingly
    /// </summary>
    private void Move()
    {
        Vector3 centerPoint = GetCenterPoint();
        centerPoint += offset;

        // update camera position
        dynamicCamera.transform.position = Vector3.SmoothDamp(dynamicCamera.transform.position, 
                                                              centerPoint, ref velocity, 0.5f);
    }

    /// <summary>
    /// gets the shortest distance from one player to another and
    /// uses that distance to determine the lerp step-size. Use said
    /// lerp to smoothly adjust the camera field of view
    /// </summary>
    private void Zoom()
    {
        // get shortest distance
        float shortestDistance = GetShortestDistance() / zoomLimiter;

        // lerp in-between min and max field of view using greatest distance
        float newZoom = Mathf.Lerp(minFieldOfView, maxFieldOfView, shortestDistance);
        dynamicCamera.fieldOfView = Mathf.Lerp(dynamicCamera.fieldOfView, newZoom, Time.deltaTime);
    }

    /// <summary>
    /// gets the x and z centers by getting the difference in x and
    /// z bounds and dividing them by 2
    /// </summary>
    /// <returns>a vector3 representing the center of all players on screen</returns>
    private Vector3 GetCenterPoint()
    {
        // get bounding box
        XZBounds bounds = GetXZBounds();

        // find center points
        float xCenter = (bounds.min.x + bounds.max.x) * 0.5f;
        float zCenter = (bounds.min.y + bounds.max.y) * 0.5f;

        // create center point and return it
        return new Vector3(xCenter, 0, zCenter);
    }

    /// <summary>
    /// gets the x and z distances by getting the difference in x and
    /// z bounds and returning the shorter distance
    /// </summary>
    /// <returns>the shortest distance from one player to another</returns>
    private float GetShortestDistance()
    {
        // get bounding box
        XZBounds bounds = GetXZBounds();

        // find x and y distances
        float xDistance = bounds.max.x - bounds.min.x;
        float zDistance = bounds.max.y - bounds.min.y;

        // return the shorter distance
        if (xDistance < zDistance)
            return xDistance;
        else
            return zDistance;
    }

    /// <summary>
    /// looks at the position of all players in scene and returns the x 
    /// and z extents for all using two pairs of x and y co-ordinates
    /// </summary>
    /// <returns>a piece of data containing two points (a rectangle, essentially)</returns>
    private XZBounds GetXZBounds()
    {
        // set minimum and maximum
        float xMin = Mathf.Infinity;
        float zMin = Mathf.Infinity;

        float xMax = Mathf.NegativeInfinity;
        float zMax = Mathf.NegativeInfinity;

        // iterate over all players
        for (int i = 0; i < players.Count; i++)
        {
            // get player
            GameObject player = players[i];

            // update minimum and maximum values as required
            if (player.transform.position.x<xMin)
                xMin = player.transform.position.x;

            if (player.transform.position.x > xMax)
                xMax = player.transform.position.x;

            if (player.transform.position.z<zMin)
                zMin = player.transform.position.z;

            if (player.transform.position.z > zMax)
                zMax = player.transform.position.z;
        }

        // create bounding box based off min & max values and return it
        return new XZBounds(xMin, xMax, zMin, zMax);
    }
}
