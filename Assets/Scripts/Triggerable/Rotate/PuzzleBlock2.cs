using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock2
//-----------------------------------------//
// The first Puzzle piece to be manipulated by a button in level 9

public class PuzzleBlock2 : Triggerable
{
    private bool isTrigger = true;
    private Quaternion nextRotation;
    private bool rotate = false;
    
    void Start()
    {
        nextRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
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
                nextRotation = nextRotation * Quaternion.Euler(0, 90, 0);
            } else {
                nextRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
            }
            rotate = true;
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}
