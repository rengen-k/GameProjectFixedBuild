using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLine : MonoBehaviour
{
    // Handles levelSet management, marks first level in open set as open
    private GameObject panalRef;
    [Tooltip("References to the set objects inside the level line.")]
    public LevelSet [] sets;
    [Tooltip("The amount of levels in this line.")]
    public int levelCount;


    void Awake()
    {
        // Say first set first level is open

        sets[0].ForceOpen();


        

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

    public void setCompletion(bool[] levelDones, bool[] levelCollectibles, int startIndex){

        int setCount = 0;
        int setLevelCount = sets[0].transform.childCount;
        int setLevelIndex = 0;

        for (int i = startIndex; i < startIndex + levelCount; i++)
        {
            bool thisLevelDone = levelDones[i];
            bool thisLevelCollectibles = levelCollectibles[i];

            if (setLevelCount == setLevelIndex)
            {
                if (sets[setCount].isDone() && setCount + 1 < sets.Length)
                {
                    sets[setCount+1].ForceOpen();
                }
                setCount++;
                setLevelCount = sets[setCount].transform.childCount;
                setLevelIndex = 0;
            }
            sets[setCount].setLevel(thisLevelDone, thisLevelCollectibles, setLevelIndex);
            setLevelIndex++;

        }


        

    }
}


    
