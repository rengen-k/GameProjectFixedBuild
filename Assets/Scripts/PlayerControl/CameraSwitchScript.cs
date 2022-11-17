using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//-----------------------------------------//
// CameraSwitchScript
//-----------------------------------------//
// Script handling the changing camera perspective.

public class CameraSwitchScript : MonoBehaviour
{
    [Tooltip("Game Object connecting Camera to input system.")]
    [SerializeField] private InputAction action;

    public int cameraPos = 0;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play("0Angle");
    }

    public int SwitchState(int direction)
    {
        //Change which virtual cam main camera will go.

        //direction is either -1 or 1, indicating whether we are going left or right.
        string[] views = {"0Angle","270Angle","180Angle","90Angle"};

        //if (checkIfAngleLock(cameraPos, direction))
        cameraPos = (cameraPos + direction) % 4;
        if (cameraPos < 0){
            cameraPos = 3;
        }
        animator.Play(views[cameraPos]);
        return cameraPos;
    }
}
