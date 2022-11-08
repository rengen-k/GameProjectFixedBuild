using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PickupItem : MonoBehaviour
{
    private PlayerActionsScript playerActionsScript;
    private Transform pickupPoint;
    private Transform player;
    private Vector3 velocity;
    public float pickupDist;
    public float throwForce;
    public bool itemPickedUp;
    public static bool ableToPickup;
    private Rigidbody rb;
    private bool reset;
    public GameObject bombPrefab;
    public bool hasBeenThrown = false;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        pickupPoint = GameObject.Find("pickupPoint").transform;
        ableToPickup = true;
    }

    private void OnEnable()
    {
        playerActionsScript = new PlayerActionsScript();
        playerActionsScript.Player.Enable();
        playerActionsScript.Player.Fire.performed += Fire;
        playerActionsScript.Player.Interact.performed += Interact;
        hasBeenThrown = false;

    }

    private void OnDisable()
    {
        playerActionsScript.Player.Disable();
    }

    void Update()
    {
        pickupDist = Vector3.Distance(player.position, transform.position);

        // for some reason it needs to be in both Update and FixedUpdate for it not to slow the player down or jiggle around
        if (itemPickedUp)
        {
            transform.position = pickupPoint.position + (transform.localScale.x * -GameObject.Find("Model").transform.forward);
        }
    }

    void FixedUpdate()
    {
        if (itemPickedUp)
        {
            transform.position = pickupPoint.position + (transform.localScale.x * -GameObject.Find("Model").transform.forward);
        }

        velocity = GameObject.Find("Player").GetComponent<Rigidbody>().velocity;

        rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
    }

    public void Interact (InputAction.CallbackContext context)
    {
        // this is so you can't pick up and drop the item on the same keystroke
        reset = true;

        // lets you drop the object if you click the interact button again
        if (itemPickedUp)
        {
            itemPickedUp = false;
            this.transform.parent = null;
            ableToPickup = true;
            rb.isKinematic = false;
            reset = false;
            //Debug.Log("Item dropped?");
        }

        // picks up the object and makes it a child of the the player's pickupPoint
        if (reset && pickupDist < 2 && !itemPickedUp && ableToPickup)
        {
            //Debug.Log("picked up");
            rb.isKinematic = true;
            transform.parent = GameObject.Find("pickupPoint").transform;
            transform.position = pickupPoint.position + (transform.localScale.x * -GameObject.Find("Model").transform.forward);
            ableToPickup = false;
            itemPickedUp = true;
        }
    }

    public void Fire (InputAction.CallbackContext context)
    {
        // throws the object based on player's current velocity
        if (itemPickedUp)
        {
            rb.isKinematic = false;
            rb.AddForce(-GameObject.Find("Model").transform.forward * throwForce + velocity, ForceMode.Impulse);
            this.transform.parent = null;
            ableToPickup = true;
            itemPickedUp = false;
            hasBeenThrown = true;
        }
    }
}
