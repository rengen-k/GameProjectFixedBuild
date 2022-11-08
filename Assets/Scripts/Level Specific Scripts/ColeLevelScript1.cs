using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColeLevelScript1 : Triggerable
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
    void Update()
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
