using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock3
//-----------------------------------------//

public class PassTag : Triggerable
{
    [SerializeField] private PuzzleBlock5 puzzleScript;

    void Start()
    {
    }

    void Update()
    {
    }

    public override void triggerAct()
    {
        string tag = transform.parent.tag;
        puzzleScript.tag = tag;
    }

}