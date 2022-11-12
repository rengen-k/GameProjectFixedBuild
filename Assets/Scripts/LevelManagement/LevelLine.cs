using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLine : MonoBehaviour
{
    // Handles levelSet management, marks first level in open set as open
    private GameObject panalRef;
    public LevelSet [] sets;


    void Start()
    {
        // Say first set first level is open

        // ASk each set if quota up, open next set
    }

    public void debugShout()
    {
        Debug.Log(panalRef.name);
    }

    public void SetRef(string panelRef)
    {
        this.panalRef = GameObject.Find(panelRef);
    }
}


    
