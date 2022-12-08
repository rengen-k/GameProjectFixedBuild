using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock1
//-----------------------------------------//
// The Second Puzzle piece to be manipulated by a button in level 9

public class PuzzleBlockZ : Triggerable
{
    private bool isTrigger = true;
    private bool rotate = false;
    private Quaternion nextRotationQuaternion;
    private Vector3 nextRotationVector;
    [SerializeField] private float nextRotationAmount;
    [SerializeField] private string axis;
    private Rigidbody rb;
    private Vector3 m_EulerAngleVelocity;
    
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        m_EulerAngleVelocity = new Vector3(0, 0, 100);
        switch (axis)
        {
            case "x":
                nextRotationVector = new Vector3(nextRotationAmount, 0, 0);
                break;
            case "y":
                nextRotationVector = new Vector3(0, nextRotationAmount, 0);
                break;
            case "z":
                nextRotationVector = new Vector3(0, 0, nextRotationAmount);
                break;
        }

        nextRotationQuaternion = transform.rotation * Quaternion.Euler(nextRotationVector);
    }

    void Update()
    {
        if (rotate) {
            transform.rotation = Quaternion.RotateTowards(transform.rotation, nextRotationQuaternion, Time.deltaTime * 40000.0f);
            if (transform.rotation == nextRotationQuaternion) {
                rotate = false;
            }
        }
    }

    // void FixedUpdate() {
    //     Quaternion deltaRotation = Quaternion.Euler(m_EulerAngleVelocity * Time.fixedDeltaTime);
    //     rb.MoveRotation(rb.rotation * deltaRotation);
    // }

    public override void triggerAct()
    {
        if (isTrigger) {
            isTrigger = false;
            Debug.Log("rotate");
            if (rotate) {
                nextRotationQuaternion = nextRotationQuaternion * Quaternion.Euler(nextRotationVector);
            } else {
                nextRotationQuaternion = transform.rotation * Quaternion.Euler(nextRotationVector);
            }
            rotate = true;
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}
