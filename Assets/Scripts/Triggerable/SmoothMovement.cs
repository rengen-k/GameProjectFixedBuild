using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmoothMovement : Triggerable
{
    // Small step that moves out when associated button is pressed.
    // Start is called before the first frame update
    private bool move = false;
    public enum Direction {
        x,
        y,
        z
    }
    [Tooltip("The axis the object will travel in when pressed.")]
    [SerializeField] private Direction d = Direction.z;
    private Vector3 oriPos;
    private Vector3 goPos;
    [Tooltip("How far out the platform will extend when pressed. Default 1")]
    [SerializeField] private int extend = 1;
     [Tooltip("Whether or not the object will return to its original position when told to.")]
    [SerializeField] private bool returny = false;

    private float speed = 2f;

    void Start()
    {
        oriPos = transform.position;
        goPos =
            new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z);
        switch (d)
        {
            case Direction.x: goPos.x += extend; break;
            case Direction.y: goPos.y += extend; break;
            case Direction.z: goPos.z += extend; break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float step =  speed * Time.deltaTime;
        if (move)
        {
            transform.position = Vector3.MoveTowards(transform.position, goPos, step);
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, oriPos, step);
        }
    }

    public override void triggerAct()
    {
        move = true;
    }

    public override void triggerUnAct()
    {
        if (returny)
        {
            move = false;
        }
    }

}
