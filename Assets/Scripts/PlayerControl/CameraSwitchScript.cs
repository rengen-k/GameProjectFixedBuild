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
    private bool canTurn = true;

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play("0Angle");
    }

    public int SwitchState(int direction)
    {
        if (!canTurn)
        {
            return cameraPos;
        }
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

    public void PlayCamera(string virtualCam)
    {
        animator.Play(virtualCam);
    }

    public void returnToPos()
    {
        // After blending to a cutscene camera, call this to return to the camera before the cutscene occured.
        string[] views = {"0Angle","270Angle","180Angle","90Angle"};
        animator.Play(views[cameraPos]);
    }

    public void PauseTurn(int pos)
    {
        string[] views = {"ViewCam", "ViewCam1", "ViewCam2"};
        canTurn = false;
        animator.Play(views[pos]);
    }

    public void UnpauseTurn()
    {
        canTurn = true;
        returnToPos();
    }
}
