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


    void Start()
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
        // given the correct bools, starting at start index, up to startindex + level count top exclusive, stuff the values inside into the correct sets, for the sets to push into the levels.
        
        int setCount = 0;
        int setLevelCount = sets[0].transform.childCount;
        int setLevelIndex = 0;
        Debug.Log("This is level line " + transform.name + " values " + startIndex);
        for (int i = startIndex; i < startIndex + levelCount; i++)
        {
            bool thisLevelDone = levelDones[i];
            bool thisLevelCollectibles = levelCollectibles[i];

            if (setLevelCount == setLevelIndex)
            {
                setCount++;
                setLevelCount = sets[setCount].transform.childCount;
                setLevelIndex = 0;
            }
            Debug.Log("This is level line " + transform.name + " setting " + sets[setCount].name + " with " + thisLevelDone + " " +  thisLevelCollectibles + " " +  setLevelIndex);
            sets[setCount].setLevel(thisLevelDone, thisLevelCollectibles, setLevelIndex);
            setLevelIndex++;

        }
    }
}


    
