using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// Level8_Button_Trigger
//-----------------------------------------//
// Triggers platform block configurations in Level 8 when a button is pressed

public class Level8_Button_Trigger : Triggerable
{
    private bool triggered;
    // alows you to specify which platforms will be visible when the level starts
    [SerializeField] private bool startActive;

    private void Start()
    {
        GetComponent<Renderer>().enabled = startActive;
        GetComponent<BoxCollider>().enabled = startActive;
    }

    // if button is pressed in level 8, the platforms that are visible alternate
    private void Update()
    {
        if (triggered)
        {
            GetComponent<Renderer>().enabled = !GetComponent<Renderer>().enabled;
            GetComponent<BoxCollider>().enabled = !GetComponent<BoxCollider>().enabled;
            triggered = false;
        }
    }

    public override void triggerAct()
    {
        triggered = true;
    }
}
