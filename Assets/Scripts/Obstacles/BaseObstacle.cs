using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseObstacle : MonoBehaviour {

    bool isDestroyable;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    /// <summary>
    /// checking if the tag is equaled to "Ram" then destroying 
    /// </summary>
    /// <param name="collision"></param>
    void OnCollisionEnter(Collision collision)
    {
      //if (collision.transform.parent.CompareTag ("Ram") || isDestroyable) <----------              
                                                                     //                |
       if (collision.gameObject.CompareTag ("Ram") || isDestroyable) //replace if this if statement does not work
       {   
            Destroy(collision.gameObject);
       }
    }

    void Destroy()
    {
        Destroy();
    }
}
