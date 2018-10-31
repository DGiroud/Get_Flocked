using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTrigger : MonoBehaviour
{

    public float explosionForce;
    public float explosionRadius;
    public float upwardsModifier;

    //This script will apply an impulse force to any object it might hit during it's descent into the playing field,
    // launching everything away from the point of impact
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sheep" || other.gameObject.tag == "Player")
        {
            Vector3 newDir;
            newDir = (transform.position - other.transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(-newDir * 20, ForceMode.Impulse);
        }
    }
}
