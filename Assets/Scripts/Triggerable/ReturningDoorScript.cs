using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// ReturningDoorScript
//-----------------------------------------//
// A door that only opens while the connected switch is pressed down.

public class ReturningDoorScript : Triggerable
{
    // Number of buttons connected to door that are currently pressed.
    private int pressedCount = 0;

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
