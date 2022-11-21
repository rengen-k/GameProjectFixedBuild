using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-----------------------------------------//
// EnemyAI
//-----------------------------------------//
// Some code from https://www.youtube.com/watch?v=c8Nq19gkNfs
// Enemy AI, handles enemy movement.

public class FrogEnemy : MonoBehaviour
{
    private Transform player;
    public Transform respawnPoint;
    private NavMeshAgent agent;
    //The list of waypoints- gameobjects, it will try to travel to.
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;
    private float originalSpeed;
    private Rigidbody rb;
    private bool grounded = true;
    private bool canJump = true;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDist;
    private Vector3 currentPos;

    [Tooltip("Indicates whether the enemy will follow the player when they are close enough.")]
    [SerializeField] private bool followsPlayer;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followPlayerDist;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        originalSpeed = agent.speed;
        UpdateDestination();
    }

    private void Update()
    {
        if (grounded && canJump)
        {
            Jump();
        }
        if (followsPlayer && Vector3.Distance(transform.position, player.position) < followPlayerDist)
        {
            agent.SetDestination(player.position);
            agent.speed = followSpeed;
        }
        else
        {
            agent.speed = originalSpeed;
            UpdateDestination();
        }
        if (Vector3.Distance(transform.position, target) < 2)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    private void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    private void IterateWaypointIndex()
    {
        waypointIndex++;
        if (waypointIndex == waypoints.Length)
        {
            waypointIndex = 0;
        }
    }

    public void Respawn()
    {
        transform.position = respawnPoint.position;
    }

    private void Jump()
    {
        grounded = false;
        if (agent.enabled)
        {
            // set the agents target to where you are before the jump
            // this stops her before she jumps. Alternatively, you could
            // cache this value, and set it again once the jump is complete
            // to continue the original move
            UpdateDestination();
            // disable the agent
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        // make the jump
        //rb.isKinematic = false;
        rb.useGravity = true;
        rb.AddForce((Vector3.up * jumpHeight) + (agent.velocity.normalized * jumpDist), ForceMode.Impulse);
        StartCoroutine(JumpCooldown());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("StableGround"))
        {
            if (!grounded)
            {
                grounded = true;
                if (agent.enabled)
                {
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    agent.isStopped = false;
                }
                //rb.isKinematic = true;
                rb.useGravity = false;
            }
        }
    }

    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(1.5f);
        canJump = true;
        UpdateDestination();
    }
}
