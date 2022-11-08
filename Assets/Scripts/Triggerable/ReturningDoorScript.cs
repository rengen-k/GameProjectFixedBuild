using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReturningDoorScript : Triggerable
{
    // A door that only opens while the connected switch is pressed down.

    // Number of buttons connected to door that are currently pressed.
    private int pressedCount = 0;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void triggerAct(){
        pressedCount++;
        if (pressedCount != 0){
            gameObject.SetActive(false);
        }

    }

    public override void triggerUnAct(){
        pressedCount--;
        if (pressedCount == 0) {
            gameObject.SetActive(true);
        }

    }
}
