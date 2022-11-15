using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level7Move : Triggerable
{
    // Small step that moves out when associated button is pressed.
    // Start is called before the first frame update
    private bool move = false;

    private Vector3 goTo;
    [Tooltip("How far out the platform will extend when pressed. Default 1")]
    [SerializeField] private int extend = 1;

    private float speed = 2f;

    void Start()
    {
        goTo =
            new Vector3(transform.position.x,
                transform.position.y,
                transform.position.z + extend);
    }

    // Update is called once per frame
    void Update()
    {
        if (move)
        {
            var step =  speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, goTo, step);
        }
    }

    public override void triggerAct()
    {
        move = true;
    }
}
