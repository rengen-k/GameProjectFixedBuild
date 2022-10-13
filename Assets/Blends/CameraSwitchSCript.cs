using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class CameraSwitchSCript : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField]

    private InputAction action;


    private Animator animator;
    void Awake()
    {
        animator = GetComponent<Animator>();
        action.Enable();
    }

    // Update is called once per frame
    void Start(){
        action.performed += _ => SwitchState();
    }
    void Update()
    {
        
    }

    private void SwitchState(){
        //Dunno how this works, need to figure out how to get the action key pressed into this function. 
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
