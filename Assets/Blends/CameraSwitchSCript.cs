using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class CameraSwitchScript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]

    private InputAction action;

    int cameraPos = 0;


    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Start(){
        
    }
    void Update()
    {
        
    }

    public void SwitchState(float direction){
        //Dunno how this works, need to figure out how to get the action key pressed into this function. 
        //direction is either -1f or 1f, indicating whether we are going left or right.
        
        animator.Play("180Angle");
        
        //if (context.ReadValue<Vector2>().x == -1f ){
        //    Debug.Log("Detected LEft movement.");
        //}
        //if (context.ReadValue<Vector2>().x == 1f ){
        //    Debug.Log("Detected Right movement.");
        //}
        animator.Play("90Angle");
    }

}
