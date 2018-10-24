using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class TriggerDisplay : MonoBehaviour {

    public enum ColliderMode { box, sphere, capsule}

    public Color mainColour = Color.red;
    public Color lineColour = Color.red;

    //Default
    public ColliderMode collider = ColliderMode.box;

    //Draws a box only visible in editor view
    void OnDrawGizmos()
	{
        switch (collider)
        {
            case ColliderMode.box:
                GetComponent<BoxCollider>().isTrigger = true;
                Vector3 drawBoxVector = new Vector3(
                    transform.lossyScale.x * GetComponent<BoxCollider>().size.x,
                    transform.lossyScale.y * GetComponent<BoxCollider>().size.y,
                    transform.lossyScale.z * GetComponent<BoxCollider>().size.z
                );

                Vector3 drawBoxPosition = transform.position + GetComponent<BoxCollider>().center;

                Gizmos.matrix = Matrix4x4.TRS(drawBoxPosition, transform.rotation, drawBoxVector);
                Gizmos.color = mainColour;
                Gizmos.DrawCube(Vector3.zero, Vector3.one);
                Gizmos.color = lineColour;
                Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
                break;

            //case ColliderMode.capsule:
            //    GetComponent<CapsuleCollider>().isTrigger = true;
            //    Vector3 drawCapsuleVector = new Vector3(
            //        transform.lossyScale.x * GetComponent<CapsuleCollider>().,
            //        transform.lossyScale.y * GetComponent<CapsuleCollider>().size.y,
            //        transform.lossyScale.z * GetComponent<CapsuleCollider>().size.z
            //    );

            //    Vector3 drawBoxPosition = transform.position + GetComponent<CapsuleCollider>().center;

            //    Gizmos.matrix = Matrix4x4.TRS(drawBoxPosition, transform.rotation, drawBoxVector);
            //    Gizmos.color = mainColour;
            //    Gizmos.DrawCube(Vector3.zero, Vector3.one);
            //    Gizmos.color = lineColour;
            //    Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
            //    break;

            case ColliderMode.sphere:
                GetComponent<SphereCollider>().isTrigger = true;
                Vector3 drawSphereVector = new Vector3(
                    transform.lossyScale.x * GetComponent<SphereCollider>().center.x,
                    transform.lossyScale.y * GetComponent<SphereCollider>().center.y,
                    transform.lossyScale.z * GetComponent<SphereCollider>().center.z
                );

                Vector3 drawSpherePosition = transform.position + GetComponent<SphereCollider>().center;

                Gizmos.matrix = Matrix4x4.TRS(drawSpherePosition, transform.rotation, drawSphereVector);
                Gizmos.color = mainColour;
                Gizmos.DrawSphere(Vector3.zero, 1);
                Gizmos.color = lineColour;
                Gizmos.DrawWireSphere(Vector3.zero, 1);
                break;
        }
	}
}
