using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveGround : Triggerable
{
    private bool isTrigger;
    [SerializeField] [Range(0f, 4f)] float lerpTime;
    [SerializeField] Vector3[] positions;
    private int posIndex = 0;
    private int length;

    private bool move;
    private float t = 0f;

    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wall3;
    [SerializeField] private GameObject wall4;

    private int pressedCount = 0;

    void Start()
    {
        length = positions.Length;
    }

    void Update()
    {
        if(move)
        {
            wall1.GetComponent<BoxCollider>().enabled = false;
            wall2.GetComponent<BoxCollider>().enabled = false;
            wall3.GetComponent<BoxCollider>().enabled = false;
            wall4.GetComponent<BoxCollider>().enabled = false;
            transform.position = Vector3.Lerp(transform.position, positions [posIndex], lerpTime * Time.deltaTime);

            t = Mathf.Lerp(t, 1f, lerpTime * Time.deltaTime);
            if(t>.9f)
            {
                t = 0f;
                posIndex++;
                posIndex = (posIndex >= length) ? 0 : posIndex;
            }
        }
    }

    public override void triggerAct()
    {
        pressedCount++;
        if (pressedCount == 2)
        {
            isTrigger = false;
            move = true;
        }
    }

    public override void triggerUnAct(){
        pressedCount--;
        if (pressedCount == 0) {
            move = false;
        }
    }
}
