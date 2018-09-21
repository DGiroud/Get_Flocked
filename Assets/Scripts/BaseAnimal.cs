using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAnimal : MonoBehaviour
{    

    private Vector3 position;

    //Base function to set the position of any inheriting animal objects
    // You've 2 options here, either pass in each dimension manually, or pass in an already created transform
    // When using either of these 2 functions, you MUST pass in the animal being changed by using the "this" keyword as the first paramater
    public void SetPosition(GameObject animal, float x, float y, float z)
    {
        animal.transform.position.Set(x, y, z); //Setting the animal's position using the passed in parameters.
    }

    //Basically the exact same as above, yet may be easier to use when scripting. Options are nice
    public virtual void SetPosition(GameObject animal, Transform transform)
    {
        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        animal.transform.position.Set(x, y, z);
    }

    //Base function to get the position of any inheriting animal objects, returns a float
    public virtual Vector3 GetPosition(GameObject animal)
    {
        return animal.transform.position;
    }

    //Destroys the object when called
    public virtual void Destroy(GameObject animal)
    {
        Destroy(animal);
    }
}
