using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VerticalRotation : AlternateTriggerable
{
    // used in level 20 to rotate blocks "vertically"
    private Transform horizontal;
    private Quaternion halfRotation = Quaternion.Euler(180, 0 , 0);
    public Quaternion newRotation;
    private AudioSource soundManager;
    public AudioClip mechanicalNoise;

    private void Start()
    {
        horizontal = GameObject.Find("Horizontal").transform;
        newRotation = Quaternion.Euler(0, 0, 0);
        soundManager = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, newRotation, Time.deltaTime * 220);
    }

    // when button is pressed, newRotation is set to either 0 or 180 degrees which is then used above to rotate blocks
    public override void triggerAct(int function)
    {
        soundManager.PlayOneShot(mechanicalNoise);
        if (function == 1 && (horizontal.rotation == Quaternion.Euler(0, 0, 0) || horizontal.rotation == Quaternion.Euler(0, 180, 0) || horizontal.rotation == Quaternion.Euler(0, -180, 0)))
        {
            if (newRotation == halfRotation)
            {
                newRotation = Quaternion.Euler(0, 0, 0);
            }
            else
            {
                newRotation = halfRotation;
            }
        }
    }
}
