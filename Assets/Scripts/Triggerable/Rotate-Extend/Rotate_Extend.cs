using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// Rotate_Extend
//-----------------------------------------//
// A level specific script that detracts, rotates, and extends one of the lens puzzle pieces

public class Rotate_Extend : Triggerable
{
    private bool isTrigger = true;
    private bool isRotate = false;
    private bool isExtend = false;
    private bool isRetract = false;

    private int lensNumber; // stores 0 or 1 or 2 to determine which lens is currently active
    private Transform lensPiece;

    // lenspiece children
    private Transform currentLens; // current active lens
    private Transform face0; // current active face0
    private Transform face1; // current active face1

    private Quaternion nextRotationQuaternion;
    private Vector3 nextRotationVector;
    private float nextRotationAmount;
    [SerializeField]private string rotateAxis;

    [Tooltip("Valid values: 1 clockwise, -1 anticlockwise")]
    [SerializeField]private int incrementAmount;

    private float faceExtendAmount = 4.0f;

    private float retractAndExtendSpeed = 6.0f;


    void Start()
    {
        nextRotationAmount = 120.0f * incrementAmount;
        lensPiece = GameObject.Find("LensPiece").transform;
        getCurrentRotationObject();

        switch (rotateAxis)
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

        nextRotationQuaternion = lensPiece.rotation * Quaternion.Euler(nextRotationVector);
    }

    // Has 3 states: retract, rotate, and extend
    private void FixedUpdate()
    {   
        if (isRetract) {
            Debug.Log("Retract");
            Vector3 originalPos = new Vector3(face0.position.x, face0.position.y, 0.0f);
            face0.position = Vector3.MoveTowards(face0.position, originalPos, retractAndExtendSpeed * Time.deltaTime);
            face1.position = Vector3.MoveTowards(face1.position, originalPos, retractAndExtendSpeed * Time.deltaTime);
            if (face0.position.z == 0.0f && face1.position.z == 0.0f) {
                isRetract = false;
                isRotate = true;
            }
        }

        if (isRotate) {
            Debug.Log("enter rotate");
            lensPiece.rotation = Quaternion.RotateTowards(lensPiece.rotation, nextRotationQuaternion, Time.deltaTime * 180.0f);
            if (lensPiece.rotation == nextRotationQuaternion) {
                isRotate = false;
                getNextRotationObjects();
                Debug.Log(lensNumber);
                isExtend = true;
            }
        }

        if (isExtend) {
            Debug.Log("Extend");
            Vector3 originalPos0 = new Vector3(face0.position.x, face0.position.y, -4.0f);
            Vector3 originalPos1 = new Vector3(face1.position.x, face1.position.y, 4.0f);

            face0.position = Vector3.MoveTowards(face0.position, originalPos0, retractAndExtendSpeed * Time.deltaTime);
            face1.position = Vector3.MoveTowards(face1.position, originalPos1, retractAndExtendSpeed * Time.deltaTime);

            if (face0.position.z <= -4.0f || face1.position.z >= 4.0f) {
                isExtend = false;
            }
        }
    }

    // Called at the beginning of the script in start(). Init all variables pertaining to rotation index and objects. 
    private void getCurrentRotationObject()
    {
        getCurrentRotationIndex();
        resetRotationObjects();
        Debug.Log("" + lensNumber);
    }

    // Once rotation Index has a value, call this to find all the gameobjects pertaining to that value.
    private void resetRotationObjects()
    {
        currentLens = lensPiece.Find("Lens" + lensNumber.ToString());
        face0 = currentLens.Find("Face0");
        face1 = currentLens.Find("Face1");
    }

    // Gets the lens number. It is determined by the rotation of lensPiece in degrees.
    private void getCurrentRotationIndex()
    {
        lensNumber = (int)(lensPiece.eulerAngles.z/ 120.0f);
    }

    // Increment/decrement the Lensnumber and assign new objects pertaining to that lensnumber.
    private void getNextRotationObjects()
    {
        incrementLensNumber();
        resetRotationObjects();
    }

    // Increment/decrement lensnumber
    private void incrementLensNumber()
    {
        lensNumber = (lensNumber + incrementAmount) % 3;
        if (lensNumber < 0) {
            lensNumber = 2;
        }
    }


    public override void triggerAct()
    {
        if (!isTrigger) {
            return;
        }
        if (isExtend || isRetract || isRotate) {
            return;
        }
        
        isTrigger = false;
        Debug.Log("rotate");
        nextRotationQuaternion = lensPiece.rotation * Quaternion.Euler(nextRotationVector);
        // if (isRotate) {
        //     nextRotationQuaternion = nextRotationQuaternion * Quaternion.Euler(nextRotationVector);
        // } else {
        //     nextRotationQuaternion = lensPiece.rotation * Quaternion.Euler(nextRotationVector);
        // }
        
        isRetract = true;
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}
