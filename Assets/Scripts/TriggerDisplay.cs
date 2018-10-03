using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerDisplay : MonoBehaviour {

    public Color mainColour = Color.red;
    public Color lineColour = Color.red;

    //Draws a box only visible in editor view
    void OnDrawGizmos()
	{
		GetComponent <BoxCollider> ().isTrigger = true;
		Vector3 drawBoxVector = new Vector3(
			this.transform.lossyScale.x * this.GetComponent<BoxCollider>().size.x,
			this.transform.lossyScale.y * this.GetComponent<BoxCollider>().size.y,
			this.transform.lossyScale.z * this.GetComponent<BoxCollider>().size.z
		);

		Vector3 drawBoxPosition = this.transform.position + this.GetComponent<BoxCollider>().center;

		Gizmos.matrix = Matrix4x4.TRS(drawBoxPosition,transform.rotation, drawBoxVector);
		Gizmos.color = mainColour;
		Gizmos.DrawCube (Vector3.zero, Vector3.one);
		Gizmos.color = lineColour;
		Gizmos.DrawWireCube (Vector3.zero, Vector3.one);
	}
}
