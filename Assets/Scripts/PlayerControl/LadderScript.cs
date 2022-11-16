using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LadderScript : MonoBehaviour
{
    private bool inLadder;
    private bool onLadder;
    [SerializeField] private float frictionAmount = 2f;
    [SerializeField] private float ladderSpeed = 10000;
    private Vector3 ladderMovement;
    private Vector2 inputVector;
    private Rigidbody rb;
    private float ladderRadius = 1.5f;
    private PlayerActionsScript playerActionsScript;
    //public CameraSwitchScript camScript;
    [SerializeField] private Transform ladderCheck;
    [SerializeField] private LayerMask whatIsLadder;
    private float velPower = 1.5f;
    private float acceleration = 17;
    private float deceleration = 25;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        inLadder = false;
        onLadder = false;
    }

    private void OnEnable()
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Interact.performed += Interact;
        //camScript = GameObject.Find("StateDrivenCamera").GetComponent<CameraSwitchScript>();
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    private void FixedUpdate()
    {
        inLadder = Physics.CheckSphere(ladderCheck.position,ladderRadius, (int) whatIsLadder);
        inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        float targetSpeed = inputVector.y * ladderSpeed;
        float speedDif = targetSpeed - rb.velocity.y;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        ladderMovement.y = move;

        if (inLadder && onLadder)
        {
            rb.AddForce(ladderMovement * Time.fixedDeltaTime);
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.y);
            rb.AddForce(Vector3.up * -amount, ForceMode.Impulse);
            //rb.MovePosition(transform.position + ladderMovement * ladderSpeed * Time.fixedDeltaTime);
        }
    }

    void Update()
    {
        if (!inLadder)
        {
            onLadder = false;
            //GetComponent<PlayerController>().enabled = true;
            rb.useGravity = true;
        }

        //if (inLadder && onLadder)
        //{
        //    ladderMovement = new Vector3(0f, inputVector.y, 0f);
        //    rb.AddForce(ladderMovement * ladderSpeed * Time.deltaTime);
        //    float amount = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(frictionAmount));
        //    amount *= Mathf.Sign(rb.velocity.y);
        //    rb.AddForce(Vector3.up * -amount, ForceMode.Impulse);
        //    //rb.MovePosition(transform.position + ladderMovement * ladderSpeed * Time.fixedDeltaTime);
        //}

        Debug.Log("inLadder: " + inLadder);
        Debug.Log("onLadder: " + onLadder);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (inLadder && !onLadder)
        {
            onLadder = true;
            //GetComponent<PlayerController>().enabled = false;
            rb.useGravity = false;
        }
        else if (onLadder)
        {
            onLadder = false;
            //GetComponent<PlayerController>().enabled = true;
            rb.useGravity = true;
        }
    }
}
