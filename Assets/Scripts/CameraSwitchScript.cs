using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraSwitchScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]

    private InputAction action;

    [SerializeField] int cameraPos = 0;

    private Animator animator;

    void Awake()
    {
        animator = GetComponent<Animator>();
        animator.Play("0Angle");

    }

    // Update is called once per frame
    void Start(){

    }
    void Update()
    {
        
    }

    public int SwitchState(int direction){
        
        //direction is either -1 or 1, indicating whether we are going left or right.

        string[] views = {"0Angle","270Angle","180Angle","90Angle"};

        //if (checkIfAngleLock(cameraPos, direction))

        

        cameraPos = (cameraPos + direction) % 4;
        if (cameraPos < 0){
            cameraPos = 3;
        }

        animator.Play(views[cameraPos]);

        Debug.Log(cameraPos);

        return cameraPos;
    }

}
