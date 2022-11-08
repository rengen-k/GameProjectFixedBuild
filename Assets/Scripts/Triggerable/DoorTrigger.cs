using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : Triggerable
{
    // A door that opens once a connected switch is pressed.

    public override void triggerAct(){
        gameObject.SetActive(false);

    }

}

