using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//-----------------------------------------//
// Bomb
//-----------------------------------------//
// Bomb, an object that can be picked up and thrown. When thrown, creates explosion that destroys objects with the destructible script.

public class Bomb : MonoBehaviour
{
    [Tooltip("Values that decide bombs explosion properties.")]
    public float delay;
    [Tooltip("Values that decide bombs explosion properties.")]
    public float blastRadius;
    [Tooltip("Values that decide bombs explosion properties.")]
    public float blastForce;

    private float countdown;

    public bool hasExploded = false;
    PickupItem pickupScript;
    public Transform respawnPoint;

    private Rigidbody rb;

    [Tooltip("References prefab that will spawn to simulate explosion")]
    [SerializeField] private GameObject explosion;
    private GameObject timer;

    private void Awake()
    {
        timer = transform.Find("WorldSpaceCanvas/BombCountdown").gameObject;
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        timer.transform.parent.GetComponent<Canvas>().worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
    }

    private void OnEnable()
    {
        countdown = delay;
        pickupScript = GetComponent<PickupItem>();
        timer.SetActive(false);
    }

    private void Update()
    {
        // To make UI timer look towards camera
        Camera camera = Camera.main;
        timer.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);

        if (pickupScript.hasBeenThrown)
        {
            timer.SetActive(true);
            countdown -= Time.deltaTime;
            timer.GetComponent<Image>().fillAmount -= 1.0f / delay * Time.deltaTime;
        }

        // IF want to make timer change colour, can set through same way fillAmount changes; timer.GetComponent<Image>().

        if (countdown <= 0f && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }

    // Find every destructible object inside explosion. Destroy it. For everything else that can move, push away.
    private void Explode()
    {
        Instantiate(explosion, transform.position, transform.rotation).GetComponent<Explosion>().bombRadious = blastRadius;

        Collider[] collidersToDestroy = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in collidersToDestroy)
        {
            Destructible dest = nearbyObject.GetComponent<Destructible>();
            if (dest != null)
            {
                dest.Destroy();
            }
        }

        Collider[] collidersToMove = Physics.OverlapSphere(transform.position, blastRadius);

        foreach (Collider nearbyObject in collidersToMove)
        {
            Rigidbody rb = nearbyObject.GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.AddExplosionForce(blastForce, transform.position, blastRadius);
            }
        }

        // Removing bomb and resetting
        ResetBomb();
    }

    private void ResetBomb()
    {
        gameObject.SetActive(false);
        transform.position = respawnPoint.position;
        gameObject.SetActive(true);
        hasExploded = false;
        timer.GetComponent<Image>().fillAmount = 1;
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.name == "KillPlane") 
        {
            rb.velocity = new Vector3(0.0f, 0.0f, 0.0f);
            ResetBomb();
        }
    }
}
