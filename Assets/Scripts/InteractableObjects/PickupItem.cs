using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

//-----------------------------------------//
// PickupItem
//-----------------------------------------//
// A script indicating that an object can be picked up with 'e' (default keybind).
public class PickupItem : MonoBehaviour
{
    private PlayerActionsScript playerActionsScript;
    private Transform pickupPoint;
    private Transform player;
    private Rigidbody playerRb;
    private Vector3 velocity;
    [Tooltip("Values indicating properties of picking up.")]
    public float pickupDist;
    [Tooltip("Values indicating properties of picking up.")]
    public float throwForce;
    [Tooltip("Values indicating properties of picking up.")]
    public bool itemPickedUp;
    [Tooltip("Value used to determine if Player can pick up object at this point in time.")]
    public static bool ableToPickup;
    private bool detectPlayer;
    private GameObject playerObj;
    public GameObject bombPrefab;
    private Rigidbody rb;
    private bool reset;
    public bool hasBeenThrown = false;

    private Vector3 respawnPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerObj = GameObject.Find("Player");
        player = GameObject.Find("Player").transform;
        playerRb = GameObject.Find("Player").GetComponent<Rigidbody>();
        pickupPoint = GameObject.Find("pickupPoint").transform;
        ableToPickup = true;
        respawnPos = transform.position;
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

    private void Update()
    {
        pickupDist = Vector3.Distance(player.position, transform.position);

        // for some reason it needs to be in both Update and FixedUpdate for it not to slow the player down or jiggle around
        if (itemPickedUp)
        {
            transform.position = pickupPoint.position + (transform.localScale.x * -GameObject.Find("Model").transform.forward);
        }
    }

    private void FixedUpdate()
    {
        if (itemPickedUp)
        {
            transform.position = pickupPoint.position + (transform.localScale.x * -GameObject.Find("Model").transform.forward);
        }

        velocity = GameObject.Find("Player").GetComponent<Rigidbody>().velocity;

        RaycastHit hit;
        rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
        if (Physics.Raycast(transform.position + new Vector3(0.0f, 0.0f, 0.0f), transform.TransformDirection(Vector3.up), out hit, 1.0f)) {
            if (hit.collider.gameObject == playerObj) {
                detectPlayer = true;
            } else {
                detectPlayer = false;
            }
        }
        // detectPlayer = Physics.Raycast(transform.position + new Vector3(0.0f, 0.0f, 0.0f), transform.TransformDirection(Vector3.up), 0.3f);
        if (detectPlayer) {
        Debug.Log(detectPlayer);
            
        }
    }

    public void Interact(InputAction.CallbackContext context)
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
        }

        // picks up the object and makes it a child of the the player's pickupPoint
        bool isPlayerVelocity0 = playerRb.velocity.y == 0;
        if (reset && pickupDist < 2 && !itemPickedUp && ableToPickup && isPlayerVelocity0 && !detectPlayer)
        {
            rb.isKinematic = true;
            transform.parent = GameObject.Find("pickupPoint").transform;
            transform.position = pickupPoint.position + (transform.localScale.x * -GameObject.Find("Model").transform.forward);
            ableToPickup = false;
            itemPickedUp = true;
        }
    }
    // throws the object based on player's current velocity
    public void Fire(InputAction.CallbackContext context)
    {
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

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "KillPlane") 
        {
            Respawn();
        }
    }

    private void Respawn()
    {
        transform.position = respawnPos;
    }
}
