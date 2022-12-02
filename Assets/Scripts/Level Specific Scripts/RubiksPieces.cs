using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksPieces : AlternateTriggerable
{
    private Transform floatingPiece;
    private Transform vertical;
    private Transform horizontal;
    private bool topRight;
    private bool verticalRotation;
    private bool horizontalRotation;

    void Start()
    {
        floatingPiece = GameObject.Find("FloatingPiece").transform;
        vertical = GameObject.Find("Vertical").transform;
        horizontal = GameObject.Find("Horizontal").transform;
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, floatingPiece.position) < 1)
        {
            topRight = true;
        }
        else
        {
            topRight = false;
        }
        if (verticalRotation && topRight)
        {
            transform.parent = vertical;
        }
        if (horizontalRotation && topRight)
        {
            transform.parent = horizontal;
        }
    }

    public override void triggerAct(int function)
    {
        if (function == 1)
        {
            verticalRotation = true;
            horizontalRotation = false;
        }
        else if (function == 2)
        {
            horizontalRotation = true;
            verticalRotation = false;
        }
    }
}
