using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// MultiSwitchDoor
//-----------------------------------------//
// Inherits Triggerable to open a door that only opens based on triggering x number of switches at the same time

public class MultiSwitchDoor : Triggerable
{
    // Number of buttons connected to door that are currently pressed.
    
    private int pressedCount = 0;
    // Number of buttons that needs to be pressed.
    [Tooltip("The number of buttons that needs to be pressed to open this door.")]
    [SerializeField] private int pressedRequire;

    public override void triggerAct(){
        pressedCount++;
        if (pressedCount == pressedRequire){
            Debug.Log("open");
            gameObject.SetActive(false);
        }
    }

    public override void triggerUnAct(){
        pressedCount--;
        if (pressedCount != pressedRequire) {
            Debug.Log("closed");
            gameObject.SetActive(true);
        }
    }
}
