using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// WaypointPath_Linear
//-----------------------------------------//
// WaypointPath that linearly traverses through points
// Source: https://www.youtube.com/watch?v=ly9mK0TGJJo

public class WaypointPath_Linear : WaypointPath
{
    // returns child object that is equal to waypoint index
    public override Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    // Given a waypoint index, increment it
    public override int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = currentWaypointIndex + 1;

        if (nextWaypointIndex == transform.childCount)
        {
            nextWaypointIndex = 0;
        }

        return nextWaypointIndex;
    }
}
