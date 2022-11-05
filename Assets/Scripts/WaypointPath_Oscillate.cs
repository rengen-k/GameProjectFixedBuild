using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Source: https://www.youtube.com/watch?v=ly9mK0TGJJo

public class WaypointPath_Oscillate : WaypointPath
{
    private bool isIncreasing = true;

    public override Transform GetWaypoint(int waypointIndex)
    {
        return transform.GetChild(waypointIndex);
    }

    public override int GetNextWaypointIndex(int currentWaypointIndex)
    {
        int nextWaypointIndex = 0;
        int maxPlatforms = transform.childCount;
        
        if (isIncreasing) {
            nextWaypointIndex = currentWaypointIndex + 1;
        } else {
            nextWaypointIndex = currentWaypointIndex - 1;
        }

        if (nextWaypointIndex == maxPlatforms) {
            isIncreasing = false;
            nextWaypointIndex -= 2;
        } else if (nextWaypointIndex == -1) {
            nextWaypointIndex += 2;
            isIncreasing = true;
        }

        return nextWaypointIndex;
    }

}
