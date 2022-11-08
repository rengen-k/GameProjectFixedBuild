using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

// some code from https://www.youtube.com/watch?v=c8Nq19gkNfs

public class EnemyAI : MonoBehaviour
{
    //Enemy AI, handles enemy movement.
    private Transform player;
    public Transform respawnPoint;
    private NavMeshAgent agent;
    //The list of waypoints- gameobjects, it will try to travel to.
    public Transform[] waypoints;
    private int waypointIndex;
    private Vector3 target;
    private float originalSpeed;

    [Tooltip("Indicates whether the enemy will follow the player when they are close enough.")]
    [SerializeField] private bool followsPlayer;
    [SerializeField] private float followSpeed;
    [SerializeField] private float followPlayerDist;

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        player = GameObject.Find("Player").transform;
        originalSpeed = agent.speed;
        UpdateDestination();
    }

    void Update()
    {
        if (followsPlayer && Vector3.Distance(transform.position, player.position) < followPlayerDist)
        {
            agent.SetDestination(player.position);
            agent.speed = followSpeed;
        }
        else if (followsPlayer)
        {
            agent.speed = originalSpeed;
            UpdateDestination();
        }
        if (Vector3.Distance(transform.position, target) < 2)
        {
            IterateWaypointIndex();
            UpdateDestination();
        }
        //Debug.Log(target);
    }

    void UpdateDestination()
    {
        target = waypoints[waypointIndex].position;
        agent.SetDestination(target);
    }

    void IterateWaypointIndex()
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
}
