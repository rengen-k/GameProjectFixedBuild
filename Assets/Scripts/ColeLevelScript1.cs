using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColeLevelScript1 : Triggerable
{
    private bool triggered;
    [SerializeField] private bool startActive;

    private void Start()
    {
        GetComponent<Renderer>().enabled = startActive;
        GetComponent<BoxCollider>().enabled = startActive;
    }

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
