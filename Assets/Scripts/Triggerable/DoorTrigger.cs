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
        gameObject.SetActive(false);

    }

}

