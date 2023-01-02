using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveRingPuzzle : Triggerable
{
    public GameObject enemy;

    public override void triggerAct()
    {
        transform.position = new Vector3(-40, 1.86f, 40.44f);
        enemy.SetActive(true);
    }

    public override void triggerUnAct()
    {
    }
}