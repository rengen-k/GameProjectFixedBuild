using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// Triggerable
//-----------------------------------------//
// Parent script scripts that need to do something when a trigger says to inherits from. Currently only used by Buttons.

public class Triggerable : MonoBehaviour
{
    
    public virtual void triggerAct(){
    }

    public virtual void triggerUnAct(){
    }
}


