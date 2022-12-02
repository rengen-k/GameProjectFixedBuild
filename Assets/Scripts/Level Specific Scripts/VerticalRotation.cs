using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotation : AlternateTriggerable
{
    private bool verticalRotation;
    private Quaternion halfRotation = Quaternion.Euler(180, 0 , 0);
    private Quaternion newRotation;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime * 5);
    }

    public override void triggerAct(int function)
    {
        if (function == 1)
        {
            newRotation = transform.rotation * halfRotation;
        }
    }
}
