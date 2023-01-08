using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class TrampolineHead : MonoBehaviour
{
    // game objects
    private NavMeshAgent agent;
    public Transform respawnPoint;
    private Transform player;

    // scaling variables
    private Vector3 squashedScale = new Vector3(1f, 0.5f, 1f);
    private Vector3 normalScale = new Vector3(1f, 1f, 1f);
    private Vector3 scale;
    private float scaleSpeed = 1.5f;

    // other variables
    private Vector3 playerBottom = new Vector3(0f, 1f, 0f);
    private bool squashed = false;

    // audio
    public AudioSource soundManager;
    public AudioClip squish;

    private void Start()
    {
        agent = GetComponentInParent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        soundManager = GameObject.Find("SoundManager").GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        if (squashed)
        {
            // makes the player have to get off the head before it becomes a trampoline
            if (Vector3.Distance(transform.position, player.position - playerBottom) > 1)
            {
                gameObject.tag = "JumpTag";
            }
        }
        // resume the NavMeshAgent and reset the tag
        else
        {
            gameObject.tag = "Ground";
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            // if the player jumps on the trampoline enemy's head, it gets squashed and the NavMeshAgent is paused
            if (!squashed)
            {
                soundManager.PlayOneShot(squish);
                squashed = true;
                scale = squashedScale;
                StartCoroutine(smoothScaling());
                agent.updateRotation = false;
                agent.isStopped = true;
                StartCoroutine(SquashCooldown());
            }
        }
        else if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }
    }

    public void Respawn()
    {
        transform.position = respawnPoint.position;
    }

    // smoothly scales the enemy up or down
    private IEnumerator smoothScaling()
    {
        while (Vector3.Distance(transform.parent.transform.localScale, scale) > 0.05f)
        {
            yield return transform.parent.transform.localScale = Vector3.Slerp(transform.parent.transform.localScale, scale, scaleSpeed * Time.fixedDeltaTime);
        }
    }

    // keeps the enemy squashed for a set amount of time and then returns it to its original scale
    private IEnumerator SquashCooldown()
    {
        squashed = true;
        yield return new WaitForSeconds(4);
        scale = normalScale;
        StartCoroutine(smoothScaling());
        squashed = false;
    }
}
