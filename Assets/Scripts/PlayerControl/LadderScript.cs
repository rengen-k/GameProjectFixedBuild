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
    private float acceleration = 30;
    private float deceleration = 40;
    RigidbodyConstraints originalConstraints;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        originalConstraints = rb.constraints;
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
        // check if we are near a ladder
        inLadder = Physics.CheckSphere(ladderCheck.position,ladderRadius, (int) whatIsLadder);

        inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();

        /* same movement as in PlayerController but for up and down. I initially tried using rb.movePosition
        but it ignored collisions so I went back to adding forces. */
        float targetSpeed = inputVector.y * ladderSpeed;
        float speedDif = targetSpeed - rb.velocity.y;
        float accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? acceleration : deceleration;
        float move = Mathf.Pow(Mathf.Abs(speedDif) * accelRate, velPower) * Mathf.Sign(speedDif);
        ladderMovement.y = move;

        /* allows you to move up and down and also applies friction if you are within distance of a ladder
        and you have clicked interact */
        if (inLadder && onLadder)
        {
            rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            rb.AddForce(ladderMovement * Time.fixedDeltaTime);
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.y), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.y);
            rb.AddForce(Vector3.up * -amount, ForceMode.Impulse);
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
    }

    // if you click the interact button and are close enough to the ladder you will be able to climb it
    public void Interact(InputAction.CallbackContext context)
    {
        if (inLadder && !onLadder)
        {
            onLadder = true;
            rb.constraints = rb.constraints = RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY;
            rb.useGravity = false;
        }
        else if (onLadder)
        {
            onLadder = false;
            rb.constraints = originalConstraints;
            rb.useGravity = true;
        }
    }
}
