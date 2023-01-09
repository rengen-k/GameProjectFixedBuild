using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// MovingPlatform
//-----------------------------------------//
// Moving Platform that uses a waypointPath object to determine locations where the platform should move to
// Source: https://www.youtube.com/watch?v=ly9mK0TGJJo

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private WaypointPath _waypointPath;
    [SerializeField] private float _speed;
    private float defaultSpeed;
    [SerializeField] private int _startingIndex;
    private GameState gameState;
    private int diff;

    private int _targetWaypointIndex;

    private Transform _previousWaypoint;
    private Transform _targetWaypoint;

    private float _timeToWaypoint;
    private float _elapsedTime;

    private void Start()
    {
        gameState = GameObject.Find("GlobalGameState").GetComponent<GameState>();
        diff = gameState.GetDifficulty();
        defaultSpeed = _speed;
        _targetWaypointIndex = _startingIndex;
        TargetNextWaypoint();
        difficultyChangeSpeed();
    }

    private void difficultyChangeSpeed()
    {
        int newDiff = gameState.GetDifficulty();
        if (newDiff == diff) {
            return;
        }
        diff = newDiff;

        switch(diff) 
        {
        case 0:
            _speed = 0.7f * _speed;
            break;
        case 1:
            _speed = defaultSpeed;
            break;
        case 2:
            _speed = 1.2f * _speed;
            break;
        }

    }

    // Move platform from previous waypoint position to target waypoint position
    private void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;

        float elapsedPercentage = _elapsedTime / _timeToWaypoint;
        elapsedPercentage = Mathf.SmoothStep(0, 1, elapsedPercentage);
        transform.position = Vector3.Lerp(_previousWaypoint.position, _targetWaypoint.position, elapsedPercentage);
        transform.rotation = Quaternion.Lerp(_previousWaypoint.rotation, _targetWaypoint.rotation, elapsedPercentage);

        if (elapsedPercentage >= 1)
        {
            TargetNextWaypoint();
        }
    }

    private void Update() {
        difficultyChangeSpeed();
    }

    // Get next waypoint (whenever elapsed percentage is 1)
    private void TargetNextWaypoint()
    {
        _previousWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);
        _targetWaypointIndex = _waypointPath.GetNextWaypointIndex(_targetWaypointIndex);
        _targetWaypoint = _waypointPath.GetWaypoint(_targetWaypointIndex);

        _elapsedTime = 0;

        float distanceToWaypoint = Vector3.Distance(_previousWaypoint.position, _targetWaypoint.position);
        _timeToWaypoint = distanceToWaypoint / _speed;
    }

    // Make an object a child whenever it is on the platform
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.transform.SetParent(transform);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            other.transform.SetParent(null);
        }
    }
}
