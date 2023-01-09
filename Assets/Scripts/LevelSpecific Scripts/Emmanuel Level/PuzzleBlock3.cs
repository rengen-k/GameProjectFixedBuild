using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// PuzzleBlock3
//-----------------------------------------//

public class PuzzleBlock3 : Triggerable
{
    [SerializeField] private GameObject cube;
    [SerializeField] private GameObject spikeSpawner;
    [SerializeField] private GameObject[] oldButtons;
    [SerializeField] private GameObject[] newButtons;
    private bool isTrigger = true;
    private bool rotate = false;
    private Quaternion nextRotation;
    private int turns = 0;
    
    void Start()
    {
        nextRotation = transform.rotation * Quaternion.Euler(0, 0, 30);
    }

    void Update()
    {
        if (turns == 5)
        {
            //spikeSpawner.SetActive(true);
            cube.SetActive(true);
            foreach (GameObject i in oldButtons)
            {
                i.SetActive(false);
            }

            foreach (GameObject j in newButtons)
            {
                j.SetActive(true);
            }
        }
        if (rotate) {
            transform.rotation = Quaternion.Slerp(transform.rotation, nextRotation, Time.deltaTime * 8);
            if (transform.rotation == nextRotation) {
                rotate = false;
            }
        }
    }

    public override void triggerAct()
    {
        if (isTrigger) {
            turns += 1;
            isTrigger = false;
            Debug.Log("rotate");
            if (rotate) {
                nextRotation = nextRotation * Quaternion.Euler(0, 0, 30);
            } else {
                nextRotation = transform.rotation * Quaternion.Euler(0, 0, 30);
            }
            rotate = true;
        }
    }

    public override void triggerUnAct()
    {
        isTrigger = true;
    }
}