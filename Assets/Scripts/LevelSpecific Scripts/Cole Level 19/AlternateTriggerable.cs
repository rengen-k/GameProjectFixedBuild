using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlternateTriggerable : MonoBehaviour
{
    // same as triggerable.cs except you can pass in an int to allow triggerable objects to know which button was triggered
    public virtual void triggerAct(int function)
    {
    }

    public virtual void triggerUnAct()
    {
    }
}


