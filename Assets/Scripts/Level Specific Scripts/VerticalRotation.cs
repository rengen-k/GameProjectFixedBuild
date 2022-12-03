using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotation : AlternateTriggerable
{
    private Transform horizontal;
    private Quaternion halfRotation = Quaternion.Euler(180, 0 , 0);
    public Quaternion newRotation;

    private void Start()
    {
        horizontal = GameObject.Find("Horizontal").transform;
        newRotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * 220);
    }

    public override void triggerAct(int function)
    {
        if (function == 1 && (horizontal.rotation == Quaternion.Euler(0, 0, 0) || horizontal.rotation == Quaternion.Euler(0, 180, 0) || horizontal.rotation == Quaternion.Euler(0, -180, 0)))
        {
            if (newRotation == halfRotation)
            {
                newRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                newRotation = halfRotation;
            }
        }
    }
}
