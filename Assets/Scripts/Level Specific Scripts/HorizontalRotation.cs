using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRotation : AlternateTriggerable
{
    private Transform vertical;
    private Quaternion halfRotation = Quaternion.Euler(0, 180, 0);
    public Quaternion newRotation;

    private void Start()
    {
        vertical = GameObject.Find("Vertical").transform;
        newRotation = Quaternion.Euler(0, 0, 0);
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * 220);
    }

    public override void triggerAct(int function)
    {
        if (function == 2 && (vertical.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0) || vertical.gameObject.transform.rotation == Quaternion.Euler(180, 0, 0) || vertical.gameObject.transform.rotation == Quaternion.Euler(-180, 0, 0)))
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