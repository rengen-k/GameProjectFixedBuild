using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// Triggerable
//-----------------------------------------//
// Parent script scripts that need to do something when a trigger says to inherits from. Currently only used by Buttons.

public class AlternateTriggerable : MonoBehaviour
{

    public virtual void triggerAct(int function)
    {
    }

    public virtual void triggerUnAct()
    {
    }
}


