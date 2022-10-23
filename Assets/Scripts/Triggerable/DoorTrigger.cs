using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Triggerable
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
 
    public override void triggerAct(){
        Debug.Log("Puzzle activated");
        transform.position = new Vector3(transform.position.x, transform.position.y - 10f, transform.position.z);

    }

}

