using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock3
//-----------------------------------------//

public class PuzzleBlock4 : Triggerable
{
    private bool isTrigger = true;
    private bool rotate = false;
    private Quaternion nextRotation;

    void Start()
    {
        nextRotation = transform.rotation * Quaternion.Euler(0, 0, -270);
    }

    void Update()
    {
        if (rotate) {
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 8);
            if (transform.rotation == nextRotation) {
                rotate = false;
            }
        }
    }

    public override void triggerAct()
    {
        if (isTrigger) {
            isTrigger = false;
            Debug.Log("rotate");
            if (rotate) {
                nextRotation = nextRotation * Quaternion.Euler(0, 0, -270);
            } else {
                nextRotation = transform.rotation * Quaternion.Euler(0, 0, -270);
            }
            rotate = true;
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}