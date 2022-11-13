using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MetaLevelSkip : MonoBehaviour
{
    // Special Script forcing a certain level to update its collectible status immediately.
    void Start()
    {

        GetComponent<GameState>().EndLevel();
    }

}
