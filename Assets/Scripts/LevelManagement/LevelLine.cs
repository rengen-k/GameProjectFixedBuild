using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLine 
{

    private GameObject panalRef;
    private LevelSet [] sets;


    public LevelLine(string panalRef)
    {
        // verify if given string is level line
        this.panalRef = GameObject.Find(panalRef);

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


    
