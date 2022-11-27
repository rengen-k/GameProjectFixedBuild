using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

//-----------------------------------------//
// EnemyAI
//-----------------------------------------//
// Some code from https://www.youtube.com/watch?v=c8Nq19gkNfs
// Enemy AI, handles enemy movement.

public class EnemyAI : MonoBehaviour
{
    // game objects
    private Transform player;
    public Transform respawnPoint;
    private NavMeshAgent agent;

    // waypoint navigation variables
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;
    private float originalSpeed;

    // variables for following player
    [SerializeField] private bool followsPlayer;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followPlayerDist;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        originalSpeed = agent.speed;
        UpdateDestination();
    }

    private void Update()
    {
        // if the enemy is set to follow the player and it is within range it will begin following at followSpeed
        if (followsPlayer && Vector3.Distance(transform.position, player.position) < followPlayerDist)
        {
            agent.SetDestination(player.position);
            agent.speed = followSpeed;
        }
        // if player moves out of range then return to patrol and original speed
        else if (followsPlayer)
        {
            agent.speed = originalSpeed;
            UpdateDestination();
        }
        // when enemy reaches a waypoint, iterates to the next one
        if (Vector3.Distance(transform.position, target) < 2)
        {
            IterateWaypointIndex();
            UpdateDestination();
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "KillPlane")
        {
            Respawn();
        }
    }
}
