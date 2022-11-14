using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaLevelSkip : MonoBehaviour
{
    // Special Script forcing a certain level to update its collectible status immediately.
    void Start()
    {
        
        GameObject obj = GameObject.Find("GlobalGameState");
        obj.GetComponent<GameState>().EndLevel();
    }

}
