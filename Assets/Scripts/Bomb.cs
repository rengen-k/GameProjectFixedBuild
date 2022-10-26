using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    public float delay;
    public float blastRadius;
    public float blastForce;
    private float countdown;
    public bool hasExploded = false;
    PickupItem pickupScript;
    public Transform respawnPoint;

    void OnEnable()
    {
        countdown = delay;
        pickupScript = GetComponent<PickupItem>();
    }

    void Update()
    {
        if (pickupScript.hasBeenThrown)
        {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0f && !hasExploded)
        {
            hasExploded = true;
            Explode();
        }
    }

    void Explode()
    {
        //this is for whenever we add an explosion effect
        // Instantiate(explosionEffect (this is a GameObject), transform.position, transform.rotation);

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

        //Destroy(gameObject);
        gameObject.SetActive(false);
        transform.position = respawnPoint.position;
        gameObject.SetActive(true);
        hasExploded = false;
    }
}
