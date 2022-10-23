using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Destructible : MonoBehaviour
{
    //we can add this later or not at all
    //public GameObject destroyedVersion;

    public void Destroy()
    {
        //we would use this line to add effects like the object breaking apart
        //Instantiate(destroyedVersion, transform.position, transform.rotation);

        Destroy(gameObject);
    }
}
