using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class LadderScript : MonoBehaviour
{
    private bool inLadder;
    private bool onLadder;
    public float ladderSpeed;
    private Vector3 ladderMovement;
    private Vector2 inputVector;
    private Rigidbody rb;
    private float ladderRadius = 0.9f;
    private PlayerActionsScript playerActionsScript;
    [SerializeField] private Transform ladderCheck;
    [SerializeField] private LayerMask whatIsLadder;

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
    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    private void FixedUpdate()
    {
        inLadder = Physics.CheckSphere(ladderCheck.position,ladderRadius, (int) whatIsLadder);
        inputVector = playerActionsScript.Player.Move.ReadValue<Vector2>();
    }

    void Update()
    {
        if (!inLadder)
        {
            onLadder = false;
            GetComponent<PlayerController>().enabled = true;
            rb.useGravity = true;
        }

        if (inLadder && onLadder)
        {
            ladderMovement = new Vector3(0f, inputVector.y, 0f);
            //rb.AddForce(ladderMovement * 1000 * Time.fixedDeltaTime);
            rb.MovePosition(transform.position + ladderMovement * ladderSpeed * Time.fixedDeltaTime);
        }
        Debug.Log("inLadder: " + inLadder);
        Debug.Log("onLadder: " + onLadder);
    }

    public void Interact(InputAction.CallbackContext context)
    {
        if (inLadder && !onLadder)
        {
            onLadder = true;
            GetComponent<PlayerController>().enabled = false;
            rb.useGravity = false;
        }
        else if (onLadder)
        {
            onLadder = false;
            GetComponent<PlayerController>().enabled = true;
            rb.useGravity = true;
        }
    }
}
