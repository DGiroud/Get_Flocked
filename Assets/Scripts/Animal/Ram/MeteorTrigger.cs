using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteorTrigger : MonoBehaviour {

    Vector2 impulse = new Vector2(0, 0);

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sheep")
        {
            Vector3 newDir;
            newDir = (transform.position - other.transform.position).normalized;
            other.GetComponent<Rigidbody>().AddForce(-newDir * 3, ForceMode.Impulse);
        }

        //Rigidbody and charactor controller don't mix
        if (other.gameObject.tag == "Player")
        {
            impulse.x = 1;
            impulse.y = 1;

            Vector3 newDir;
            //current - destination
            newDir = (transform.position - other.transform.position).normalized;

            newDir.x += impulse.x;
            newDir.z += impulse.y;

            other.GetComponent<BaseActor>().Move(-newDir.x, -newDir.z);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            if (impulse.x > 0 && impulse.y > 0)
            {
                impulse.x -= 0.2f;
                impulse.y -= 0.2f;
            }

            Vector3 newDir;
            newDir = (transform.position - other.transform.position).normalized;

            newDir.x += impulse.x;
            newDir.z += impulse.y;

            other.GetComponent<BaseActor>().Move(-newDir.x, -newDir.z);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "player")
        {
            Destroy(other.GetComponent<Rigidbody>());
        }
    }
}
