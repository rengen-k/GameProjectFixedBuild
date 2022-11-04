using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    // Start is called before the first frame update

    //set this to true in unity editor to indicate button will not rise when nothing pushes it.
    public bool isStuck = false;

    private bool press;
    [SerializeField] SpringJoint spring;

    //Put in the inspector of the button, the object with the triggerable child script to run when the button is pressed.
    [SerializeField] Triggerable[] trigger;
    [SerializeField] bool playnt;

    void Start()
    {
        if (isStuck){
            spring.breakForce = 10;
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //trigger catch here.

    private void OnTriggerEnter(Collider other)
    {
        if (playnt){
            if (other.name == "Player"){
                return;
                }
        }
        //Debug.Log("Trigger triggered!" + other.name);
        foreach (Triggerable t in trigger)
        {
            t.triggerAct();
        }
    }

    private void OnTriggerExit(Collider other){

        foreach (Triggerable t in trigger)
        {
            t.triggerUnAct();
        }
    }
}
