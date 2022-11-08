using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MultiSwitchDoor : Triggerable
{
    // Number of buttons connected to door that are currently pressed.
    
    private int pressedCount = 0;
    // Number of buttons that needs to be pressed.
    [Tooltip("The number of buttons that needs to be pressed to open this door.")]
    [SerializeField] private int pressedRequire;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void triggerAct(){
        pressedCount++;
        if (pressedCount == pressedRequire){
            Debug.Log("open");
            gameObject.SetActive(false);
        }

    }

    public override void triggerUnAct(){
        pressedCount--;
        if (pressedCount != pressedRequire) {
            Debug.Log("closed");
            gameObject.SetActive(true);
        }

    }

}