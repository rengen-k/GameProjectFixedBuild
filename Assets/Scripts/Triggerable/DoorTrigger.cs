using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// DoorTrigger
//-----------------------------------------//
// Inherits Triggerable to open a door once switch is pressed

public class DoorTrigger : Triggerable
{
    // A door that opens once a connected switch is pressed.
    public override void triggerAct(){
        gameObject.SetActive(false);
    }
}

