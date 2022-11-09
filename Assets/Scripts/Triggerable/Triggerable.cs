using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//-----------------------------------------//
// WaypointPath_Linear
//-----------------------------------------//
// A door that only opens while the connected switch is pressed down.
public class Triggerable : MonoBehaviour
{
    // Parent script scripts that need to do something when a trigger says to inherits from. Currently only used by Buttons.
    public virtual void triggerAct(){
    }

    public virtual void triggerUnAct(){
    }
}


