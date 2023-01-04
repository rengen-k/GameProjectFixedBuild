using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
//using static UnityEditor.Experimental.GraphView.GraphView;

public class FrogEnemy : MonoBehaviour
{
    // game objects and components
    private Transform player;
    private Transform stuckObject;
    public Transform respawnPoint;
    private NavMeshAgent agent;
    private Rigidbody rb;

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
    [SerializeField] private LayerMask layerMask;
    private Ray ray;
    private RaycastHit hit;
    private float rayDist = 10;
    private Ray tongueRay;
    private RaycastHit tongueHit;
    private float tongueDist = 1;
    private Vector3 endPoint;
    private float attackRange = 10;
    public bool finishedAttack = true;
    public bool canAttack = false;
    public bool attackCooldown = false;
    public bool stuckToObject = false;

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

        if (stuckToObject)
        {
            stuckObject.position = transform.GetChild(0).transform.position;
        }

        if (canAttack && !attackCooldown)
        {
            agent.updatePosition = false;
            agent.updateRotation = false;
            agent.isStopped = true;
        }
        else if (finishedAttack && grounded)
        {
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.isStopped = false;
        }

        if (grounded && canJump && finishedAttack)
        {
            Jump();
        }

        if (canAttack && finishedAttack && !attackCooldown && grounded)
        {
            Attack();
        }

        if (followsPlayer && Vector3.Distance(transform.position, player.position) < followPlayerDist)
        {
            // follows player and can change the speed if desired
            agent.SetDestination(player.position);
            agent.speed = followSpeed;
            // gets the endpoint for the raycast in TongueMovement() and initializes the ray passed into the next raycast
            if (agent.velocity != Vector3.zero && !stuckToObject)
            {
                endPoint = transform.position + (agent.velocity.normalized * attackRange);
                ray = new Ray(transform.position, agent.velocity.normalized);
            }
            // frog is able to attack when a raycast of a capped distance hits the player
            if (Physics.Raycast(ray: ray, hitInfo: out hit, maxDistance: rayDist, layerMask: ~layerMask) && hit.transform.gameObject.tag == "Player")
            {
                Debug.Log("ray hit player");
                canAttack = true;
            }
        }
        // when not in range of player, frog returns to its patrol
        else if (followsPlayer)
        {
            canAttack = false;
            agent.speed = originalSpeed;
            UpdateDestination();
        }

        // when enemy reaches a waypoint, iterates to the next one
        if (Vector3.Distance(transform.position, target) < 2)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }

        FaceTarget();
    }

    private void FixedUpdate()
    {
        // increases gravity slightly
        if (rb.useGravity == true)
        {
            rb.AddForce(Physics.gravity * 1.2f, ForceMode.Acceleration);
        }
    }

    // allows enemy to patrol a set path of waypoints
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
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;
        waypointIndex = 0;
        FaceTarget();
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
        rb.AddForce((Vector3.up * jumpHeight) + (agent.velocity.normalized * jumpDist), ForceMode.Impulse);
        StartCoroutine(JumpCooldown());
    }

    // frog uses it's tongue to try and stick to the player
    private void Attack()
    {
        finishedAttack = false;
        // pauses the NavMeshAgent while attacking
        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.isStopped = true;
        StartCoroutine(TongueMovement());
        StartCoroutine(AttackCooldown());
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
        // allows the enemy to phase through pickup items so it isn't always stuck
        if (collision.gameObject.tag == "PickupObject")
        {
            Physics.IgnoreCollision(collision.collider, GetComponent<Collider>(), true);
        }
    }

    /* from https://forum.unity.com/threads/how-do-i-update-the-rotation-of-a-navmeshagent.707579/ makes the enemy look 
    the direction it is moving in which is required because the NavMeshAgent is frequently "paused" throughout the code
    so its rotation has to be handled manually */
    private void FaceTarget()
    {
        Vector3 turnTowardNavSteeringTarget = agent.steeringTarget;
        Vector3 direction = (turnTowardNavSteeringTarget - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5);
    }

    // controls how often the enemy jumps
    private IEnumerator JumpCooldown()
    {
        canJump = false;
        yield return new WaitForSeconds(jumpTime);
        canJump = true;
    }

    // controls how often the enemy can attack
    private IEnumerator AttackCooldown()
    {
        attackCooldown = true;
        yield return new WaitForSeconds(5);
        attackCooldown = false;
    }

    /* coroutine that extends and retracts the frog's tongue and also uses raycasts to determine whether it has run into
    the player or a pickup item along the way so it can "grab" them */
    private IEnumerator TongueMovement()
    {
        finishedAttack = false;
        // tongue extension
        while (Vector3.Distance(transform.GetChild(0).transform.position, endPoint) > 0.7)
        {
            tongueRay = new Ray(transform.GetChild(0).transform.position, endPoint);
            if (Physics.Raycast(tongueRay, out tongueHit, tongueDist) && (tongueHit.transform.gameObject.tag == "Player" || tongueHit.transform.gameObject.tag == "PickupObject"))
            {
                stuckObject = tongueHit.transform;
                endPoint = tongueHit.transform.position;
                stuckToObject = true;
            }
            transform.GetChild(0).transform.position = Vector3.MoveTowards(transform.GetChild(0).transform.position, endPoint, 0.03f);
            yield return null;
        }
        // tongue retraction
        while (Vector3.Distance(transform.GetChild(0).transform.position, transform.position) > 0.7)
        {
            transform.GetChild(0).transform.position = Vector3.MoveTowards(transform.GetChild(0).transform.position, transform.position, 0.03f);
            yield return null;
        }
        stuckToObject = false;
        finishedAttack = true;
    }
}
