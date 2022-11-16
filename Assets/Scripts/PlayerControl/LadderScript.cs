using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LadderScript : MonoBehaviour
{
    public bool inLadder;
    public bool onLadder;
    [SerializeField] private float frictionAmount = 2f;
    [SerializeField] private float ladderSpeed = 10000;
    private Vector3 ladderMovement;
    private Vector2 inputVector;
    private Rigidbody rb;
    private float ladderRadius = 1.25f;
    private PlayerActionsScript playerActionsScript;
    //public CameraSwitchScript camScript;
    [SerializeField] private Transform ladderCheck;
    [SerializeField] private LayerMask whatIsLadder;
    private float velPower = 1.5f;
    private float acceleration = 30;
    private float deceleration = 40;
    RigidbodyConstraints originalConstraints;
    private PlayerController playerController;
    private Vector3 tempPos;
    private float offset = 0.6f;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalConstraints = rb.constraints;
        inLadder = false;
        onLadder = false;
        playerController = GetComponent<PlayerController>();
    }

    private void OnEnable()
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        //camScript = GameObject.Find("StateDrivenCamera").GetComponent<CameraSwitchScript>();
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    private void FixedUpdate()
    {
        // check if we are near a ladder
        inLadder = Physics.CheckSphere(ladderCheck.position,ladderRadius, (int) whatIsLadder);

        inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        /* same movement as in PlayerController but for up and down. I initially tried using rb.movePosition
        but it ignored collisions so I went back to adding forces */
        float targetSpeed = inputVector.y * ladderSpeed;
        float speedDif = targetSpeed - rb.velocity.y;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        ladderMovement.y = move;

        // says you are on the ladder if you are within range and have clicked the up or down input
        if (inLadder && inputVector.y != 0)
        {
            onLadder = true;
            tempPos = transform.position;
            Collider[] ladders = Physics.OverlapSphere(ladderCheck.position, ladderRadius, (int)whatIsLadder);
            //if (ladders[0] != null)
            //{
            //    if (ladders[0].transform.rotation == Quaternion.Euler(0, 0, 0) && transform.position.x < ladders[0].transform.position.x)
            //    {
            //        tempPos.x = ladders[0].transform.position.x - offset;
            //        transform.position = tempPos;
            //    }
            //    else if (ladders[0].transform.rotation == Quaternion.Euler(0, 90, 0) && transform.position.z < ladders[0].transform.position.z)
            //    {
            //        tempPos.z = ladders[0].transform.position.z - offset;
            //        transform.position = tempPos;
            //    }
            //    else if (ladders[0].transform.rotation == Quaternion.Euler(0, 180, 0) && transform.position.x > ladders[0].transform.position.x)
            //    {
            //        tempPos.x = ladders[0].transform.position.x + offset;
            //        transform.position = tempPos;
            //    }
            //    else if (ladders[0].transform.rotation == Quaternion.Euler(0, -90, 0) && transform.position.z > ladders[0].transform.position.z)
            //    {
            //        tempPos.z = ladders[0].transform.position.z + offset;
            //        transform.position = tempPos;
            //    }

            //}

            foreach (Collider ladder in ladders)
            {
                if (ladder.transform.rotation == Quaternion.Euler(0, 0, 0) && transform.position.x < ladder.transform.position.x)
                {
                    tempPos.x = ladder.transform.position.x - offset;
                    transform.position = tempPos;
                }
                else if (ladder.transform.rotation == Quaternion.Euler(0, 90, 0) && transform.position.z < ladder.transform.position.z)
                {
                    tempPos.z = ladder.transform.position.z - offset;
                    transform.position = tempPos;
                }
                else if (ladder.transform.rotation == Quaternion.Euler(0, 180, 0) && transform.position.x > ladder.transform.position.x)
                {
                    tempPos.x = ladder.transform.position.x + offset;
                    transform.position = tempPos;
                }
                else if (ladder.transform.rotation == Quaternion.Euler(0, -90, 0) && transform.position.z > ladder.transform.position.z)
                {
                    tempPos.z = ladder.transform.position.z + offset;
                    transform.position = tempPos;
                }
            }
        }

        /* allows you to move up and down and also applies friction if you are within distance of a ladder
        and you have clicked up or down */
        if (inLadder && onLadder && !playerController.isJumping)
        {
            rb.AddForce(ladderMovement * Time.fixedDeltaTime);
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.y);
            rb.AddForce(Vector3.up * -amount, ForceMode.Impulse);

            //tempPos = transform.position;
            //Collider[] ladders = Physics.OverlapSphere(ladderCheck.position, ladderRadius, (int)whatIsLadder);
            //if (ladders[0] != null)
            //{
            //    if (ladders[0].transform.rotation == Quaternion.Euler(0, 0, 0) && transform.position.x < ladders[0].transform.position.x)
            //    {
            //        tempPos.x = ladders[0].transform.position.x - offset;
            //        transform.position = tempPos;
            //    }
            //    else if (ladders[0].transform.rotation == Quaternion.Euler(0, 90, 0) && transform.position.z < ladders[0].transform.position.z)
            //    {
            //        tempPos.z = ladders[0].transform.position.z - offset;
            //        transform.position = tempPos;
            //    }
            //    else if (ladders[0].transform.rotation == Quaternion.Euler(0, 180, 0) && transform.position.x > ladders[0].transform.position.x)
            //    {
            //        tempPos.x = ladders[0].transform.position.x + offset;
            //        transform.position = tempPos;
            //    }
            //    else if (ladders[0].transform.rotation == Quaternion.Euler(0, -90, 0) && transform.position.z > ladders[0].transform.position.z)
            //    {
            //        tempPos.z = ladders[0].transform.position.z + offset;
            //        transform.position = tempPos;
            //    }
            //}
        }
    }

    void Update()
    {
        // if you move out of range of a ladder, you will not be able to continue moving up and down
        if (!inLadder)
        {
            onLadder = false;
            rb.constraints = originalConstraints;
            rb.useGravity = true;
        }
        // turns off gravity while on a ladder so you don't slide down
        if (onLadder)
        {
            rb.useGravity = false;
            ladderRadius = 0.8f;
        }
        else
        {
            ladderRadius = 1.25f;
        }
    }
}
