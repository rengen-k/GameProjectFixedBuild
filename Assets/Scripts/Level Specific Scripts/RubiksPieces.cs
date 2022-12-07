using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubiksPieces : AlternateTriggerable
{
    /* this script is used in level 20 by each of the four blocks that make up the level. Determines which blocks rotate
    after every button press */

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
        // indicates if the block is in the top right position (opposite the stationary block)
        if (Vector3.Distance(transform.position, floatingPiece.position) < 0.1)
        {
            topRight = true;
        }
        else
        {
            topRight = false;
        }
        // changes parent of the block in the top right because it can be rotated along the y or x axis
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
