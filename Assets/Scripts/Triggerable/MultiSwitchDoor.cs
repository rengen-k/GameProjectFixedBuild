using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

//-----------------------------------------//
// MultiSwitchDoor
//-----------------------------------------//
// Inherits Triggerable to open a door that only opens based on triggering x number of switches at the same time

public class MultiSwitchDoor : Triggerable
{
    // Number of buttons connected to door that are currently pressed.
    
    private int pressedCount = 0;
    // Number of buttons that needs to be pressed.
    [Tooltip("The number of buttons that needs to be pressed to open this door.")]
    [SerializeField] private int pressedRequire;

    [Tooltip("How low the door will go before becoming inactive.")]
    [SerializeField] private int extend;

    private bool open = false;
    private bool setOpen;
    private bool pauseMovement = false;
    private bool blending;

    private Vector3 oriPos;
    private Vector3 newPos;

    private CinemachineBrain cam;
    private PlayerController pc;
    private Animator animator;


    void Start()
    {
        newPos = oriPos = transform.position;
        newPos.y -= extend;

        cam = GameObject.Find("Main Camera").GetComponent<CinemachineBrain>();
        pc = GameObject.Find("Player").GetComponent<PlayerController>();
        animator = GameObject.Find("StateDrivenCamera").GetComponent<Animator>();
    }

    void Update()
    {
        if (blending)
        {
            if (!cam.IsBlending)
            {
                blending = false;
                open = setOpen;
            }
        }
        if (pauseMovement)
        {
            Vector3 pos;
            if (open)
            {
                pos = newPos;
            }
            else 
            {
                pos = oriPos;
            }
            float step =  2.75f * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, pos, step);
                
            if (transform.position == pos)
            {
                pauseMovement = false;
                pc.Enable();
                GameObject.Find("StateDrivenCamera").GetComponent<CameraSwitchScript>().returnToPos();
            }
            if (setOpen && !pauseMovement)
            {
                gameObject.SetActive(false);
            }
        }


    }

    public override void triggerAct(){
        pressedCount++;
        if (pressedCount == pressedRequire){
            pc.Disable();

            setOpen = true;
            pauseMovement = true;
            blending = true;

            animator.Play("DoorCam");

        }
    }

    public override void triggerUnAct(){
        pressedCount--;
        if (pressedCount != pressedRequire && open) {
            gameObject.SetActive(true);
            pc.Disable();
            setOpen = false;
            pauseMovement = true;
            blending = true;
            animator.Play("DoorCam");
        }
    }
}
