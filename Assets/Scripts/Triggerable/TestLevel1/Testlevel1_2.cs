using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Testlevel1_2 : Triggerable
{
    private bool isTrigger = true;
    private Quaternion nextRotation;
    private bool rotate = false;

    public float speed = 0.1f; //speed in degrees per second?
    
    // Start is called before the first frame update
    void Start()
    {
        nextRotation = transform.rotation * Quaternion.Euler(0, 90, 0);
    }

    // Update is called once per frame
    void Update()
    {
        // transform.rotation = transform.rotation * Quaternion.Euler(0, 90, 0);

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
            // Debug.Log(nextRotation);
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}
