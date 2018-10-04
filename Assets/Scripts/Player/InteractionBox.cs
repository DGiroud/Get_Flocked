using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionBox : MonoBehaviour
{
    /// <summary>
    /// handle interaction box collision
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        // if the object is a sheep...
        if (other.CompareTag("Sheep"))
        {
            BaseActor parentScript = GetComponentInParent<BaseActor>();

            // ...snap the sheep to actor
            parentScript.SnapSheep(other.gameObject);
        }
    }
}
