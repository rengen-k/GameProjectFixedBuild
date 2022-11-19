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
    private Vector3 velocity;
    [Tooltip("Values indicating properties of picking up.")]
    public float pickupDist;
    [Tooltip("Values indicating properties of picking up.")]
    public float throwForce;
    [Tooltip("Values indicating properties of picking up.")]
    public bool itemPickedUp;
    [Tooltip("Value used to determine if Player can pick up object at this point in time.")]
    public static bool ableToPickup;

    public GameObject bombPrefab;
    private Rigidbody rb;
    private bool reset;
    public bool hasBeenThrown = false;
    private bool inWater = false;
    [SerializeField] private LayerMask whatIsWater;
    private float waterRadius;

    private Vector3 respawnPos;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        player = GameObject.Find("Player").transform;
        pickupPoint = GameObject.Find("pickupPoint").transform;
        ableToPickup = true;
        respawnPos = transform.position;
        waterRadius = transform.localScale.x / 3;
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

        inWater = Physics.CheckSphere(transform.position, waterRadius, (int)whatIsWater);

        // falls slower in water
        if (inWater)
        {
            rb.drag = 3;
        }
        else
        {
            rb.drag = 0;
            rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
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
        if (reset && pickupDist < 2 && !itemPickedUp && ableToPickup)
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
        //Debug.Log("collision");
        //if (other.gameObject.layer == 1 << 4)
        //{
        //    Debug.Log("in water");
        //    inWater = true;
        //}
    }

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
    //    {
    //        Debug.Log("in water");
    //        inWater = true;
    //    }
    //}

    //private void OnTriggerExit(Collider other)
    //{
    //    if (other.gameObject.layer == LayerMask.NameToLayer("Water"))
    //    {
    //        Debug.Log("out of water");
    //        inWater = false;
    //    }
    //}

    private void Respawn()
    {
        transform.position = respawnPos;
    }
}
