using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FrogEnemy : MonoBehaviour
{
    // game objects and components
    private Transform player;
    public Transform respawnPoint;
    private NavMeshAgent agent;
    private Rigidbody rb;
    private Ray ray;
    private RaycastHit hit;
    private float rayDist = 10;
    private Vector3 endPoint;
    private float attackRange = 10;
    public bool finishedAttack = true;
    private Vector3 startingPos;
    public bool canAttack = false;

    //The list of waypoints - gameobjects, it will try to travel to.
    public Transform[] waypoints;

    // general enemy variables
    private int waypointIndex;
    private Vector3 target;
    private float originalSpeed;

    // frog related variables
    private bool grounded = true;
    private bool canJump = true;
    [SerializeField] private float jumpHeight;
    [SerializeField] private float jumpDist;
    [SerializeField] private float jumpTime;

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
        // stops the agent teleporting backwards after jumping
        if (agent.updatePosition == false)
        {
            agent.nextPosition = transform.position;
        }
        if (canAttack)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        else if (!canAttack && grounded)
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;
        }
        if (grounded && canJump && finishedAttack)
        {
            Jump();
        }
        if (canAttack && finishedAttack)
        {
            Attack();
        }
        // increases speed when following player
        if (followsPlayer && Vector3.Distance(transform.position, player.position) < followPlayerDist)
        {
            agent.SetDestination(player.position);
            agent.speed = followSpeed;
            if (agent.velocity != Vector3.zero)
            {
                endPoint = transform.position + (agent.velocity.normalized * attackRange);
                ray = new Ray(transform.position, agent.velocity.normalized);
            }
            if (Physics.Raycast(ray, out hit, rayDist) && hit.transform.gameObject.tag == "Player")
            {
                canAttack = true;
            }
        }
        else if (followsPlayer)
        {
            canAttack = false;
            agent.speed = originalSpeed;
            UpdateDestination();
        }
        if (Vector3.Distance(transform.position, target) < 2)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
    }

    private void FixedUpdate()
    {
        // increases gravity slightly
        if (rb.useGravity == true)
        {
            rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
        }
    }

    private void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    // called when enemy approaches a waypoint so it can continue to the next one
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
            // deactivates NavMeshAgent to allow it to jump
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        rb.useGravity = true;

        // jumps up and in the direction the agent was moving prior to being deactivated
        //if (agent.velocity != Vector3.zero)
        //{
        //    endPoint = transform.position + (agent.velocity.normalized * attackRange);
        //}
        rb.AddForce((Vector3.up * jumpHeight) + (agent.velocity.normalized * jumpDist), ForceMode.Impulse);
        StartCoroutine(JumpCooldown());
    }

    private void Attack()
    {
        finishedAttack = false;
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.isStopped = true;
        startingPos = transform.GetChild(0).transform.position;
        //endPoint = transform.position + (agent.velocity.normalized * attackRange);
        //Debug.Log("Endpoint in attack(): " + endPoint);
        StartCoroutine(TongueMovement());
    }

    private void OnCollisionEnter(Collision collision)
    {
        // ground detection
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground") || collision.gameObject.layer == LayerMask.NameToLayer("StableGround"))
        {
            if (!grounded)
            {
                grounded = true;
                if (agent.enabled)
                {
                    // reactivates NavMeshAgent once on the ground
                    agent.updatePosition = true;
                    agent.updateRotation = true;
                    agent.isStopped = false;
                }
                rb.useGravity = false;
            }
        }
        else if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }
    }

    // controls how often the enemy jumps
    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpTime);
        canJump = true;
    }

    private IEnumerator AttackCooldown()
    {
        canAttack = false;
        yield return new WaitForSeconds(4);
        canAttack = true;
    }

    private IEnumerator TongueMovement()
    {
        finishedAttack = false;
        //endPoint = transform.position + (agent.velocity.normalized * attackRange);
        while (Vector3.Distance(transform.GetChild(0).transform.position, endPoint) > 0.7)
        {
            transform.GetChild(0).transform.position = Vector3.MoveTowards(transform.GetChild(0).transform.position, endPoint, 0.02f);
            yield return null;
        }
        while (Vector3.Distance(transform.GetChild(0).transform.position, transform.position) > 0.7)
        {
            transform.GetChild(0).transform.position = Vector3.MoveTowards(transform.GetChild(0).transform.position, transform.position, 0.02f);
            yield return null;
        }
        finishedAttack = true;
    }
}
