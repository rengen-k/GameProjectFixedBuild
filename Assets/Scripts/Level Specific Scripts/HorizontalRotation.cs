using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HorizontalRotation : AlternateTriggerable
{
    // used in level 20 to rotate blocks "horizontally"
    private Transform vertical;
    private Quaternion halfRotation = Quaternion.Euler(0, 180, 0);
    public Quaternion newRotation;
    private AudioSource soundManager;
    public AudioClip mechanicalNoise;

    private void Start()
    {
        vertical = GameObject.Find("Vertical").transform;
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
        if (function == 2 && (vertical.gameObject.transform.rotation == Quaternion.Euler(0, 0, 0) || vertical.gameObject.transform.rotation == Quaternion.Euler(180, 0, 0) || vertical.gameObject.transform.rotation == Quaternion.Euler(-180, 0, 0)))
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