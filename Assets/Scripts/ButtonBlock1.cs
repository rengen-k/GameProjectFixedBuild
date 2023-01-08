using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBlock1 : MonoBehaviour
{
    // Start is called before the first frame update

    //set this to true in unity editor to indicate button will not rise when nothing pushes it.
    [Tooltip("Boolean expessing if the button will not spring back when nothing holds it down.")]
    public bool isStuck = false;

    [Tooltip("The spring attached to the button, if the button stays down once pressed, the spring is set to break.")]
    [SerializeField] SpringJoint spring;

    //Put in the inspector of the button, the object with the triggerable child script to run when the button is pressed.
    [Tooltip("A list of the objects that change when the button gets pressed. Needs a script that is the child of Triggerable.cs")]
    [SerializeField] Triggerable[] trigger;
    [Tooltip("Whether or not the player can push the button down with themselves, if true, have to use an object to press it down.")]
    [SerializeField] bool playnt;
    public string col;

    void Start()
    {
        if (isStuck){
            spring.breakForce = 20;
        }

    }

    // Update is called once per frame
    void Update()
    {

    }


    private void OnTriggerEnter(Collider other)
    {
        col = other.name;
        if (playnt){
            if (other.name == "Block1" | other.name == "Block2"){
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
        if (playnt){
            if (other.name == "Block1" | other.name == "Block2"){
                return;
                }
        }

        foreach (Triggerable t in trigger)
        {
            t.triggerUnAct();
        }
    }


}