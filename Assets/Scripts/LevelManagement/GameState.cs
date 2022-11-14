using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


//-----------------------------------------//
// GameState
//-----------------------------------------//
// Singleton that keeps track of which levels are completed, and which levels should be asseccible. 

// TODO 
// Organise
public class GameState : MonoBehaviour
{
    public static GameState instance;

    //Level completion management
    // Set range to (0, X+1) where X is the number of levels
    private static int [] levelList = Enumerable.Range(0, 11).ToArray();
    private bool [] levelDones;
    // Reference Array to each LevelLine Object, automatically generated.
    private LevelLine [] lines;

    private string [] levelLineNames = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};

    // Collectible Management
    private bool [] levelsCollected;
    private GameObject msg;
    //Global counter, updates on end level
    [Tooltip("The total collectibles collected this game session, updates at the end of a level, assuming all has been retrieved.")]
    public int totalCollectibles = 0;
    private int thisLevelTotalCollectibles;
    private int thisLevelCollectibles = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        
    }

    //Setting up.

    void Start()
    {
        if (instance != null && instance != this)
        {
            instance.ResetSingleton();
            instance.UpdateLevelLines();
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            SetUp();
            ResetSingleton();
            UpdateLevelLines();
        }
        //lines[0].debugShout();

    }


    private void SetUp()
    {      
        levelDones = new bool[levelList.Length];
        levelsCollected = new bool[levelList.Length];
    }

    private void ResetSingleton()
    {
        //Sets references to loaded scene, resets counter.
        // TODO, make this method pack GameState.lines with the correct references to the canvas.
        // Every canvas element with the script LevelLine, specifically. 
        GameObject [] linesObj = GameObject.FindGameObjectsWithTag("LevelLine");
        lines = linesObj.Select( line => line.GetComponent<LevelLine>()).ToArray();
        
        msg = GameObject.Find("CollectibleNotify");
        thisLevelCollectibles = 0;


    }

    // Passing data down, updating panal.

    private void UpdateLevelLines()
    {
        // Updates GameState references to refer to the current scenes objects. And then passes down data to those objects
        
        msg.SetActive(false);

        //Gives level lines the correct panel ref, sets each level line to value from varaibles levelDones
        int startIndex = 0;
        for (int i = 0; i < levelLineNames.Length; i++)
        {
            //TODO, when lines is filled automatically, no need for next line.
            lines[i].setCompletion(levelDones, levelsCollected, startIndex);
            startIndex += lines[i].levelCount;
        }

        thisLevelTotalCollectibles =
            GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

   

    // Updating attributes

    public void EndLevel()
    {
        int levelNum =
            Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
        if (thisLevelCollectibles == thisLevelTotalCollectibles)
        {            
            levelsCollected[levelNum] = true;
            totalCollectibles += thisLevelCollectibles;
        }
        thisLevelCollectibles = 0;
        levelDones[levelNum] = true;
        int startIndex = 0;
        foreach (LevelLine l in lines)
        {
            l.setCompletion(levelDones, levelsCollected, startIndex);
            startIndex += l.levelCount;
        }

    }

    public void Collected()
    {
        thisLevelCollectibles++;
        if (thisLevelCollectibles == thisLevelTotalCollectibles)
        {
            msg.SetActive(true);
        }
    }
    
    public static int LevelCount ()
    {
        return levelList.Length;
    }


    
}
