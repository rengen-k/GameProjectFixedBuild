using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock3
//-----------------------------------------//

public class PassTag3 : Triggerable
{
    [SerializeField] private PuzzleBlock7 puzzleScript;

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