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
// CollectibleTracker in here.
// 
// Handle end of level events- yell at the correct levelLines
//When levelloader touched, mark level complete in gamestate, load map menu,

public class GameState : MonoBehaviour
{
    public static GameState instance;

    //Level completion management
    // Set range to (0, X+1) where X is the number of levels
    private static int [] levelList = Enumerable.Range(0, 11).ToArray();
    private bool [] levelDones;
    private GameObject levelPanel;
    private LevelLine [] lines;

    // Collectible Management
    private bool [] levelsCollected;
    private GameObject msg;
    private int thisLevelCollectibles;
    

    

    // Start is called before the first frame update
    void Awake()
    {
        
    }

    //Setting up.

    void Start()
    {
        if (instance != null && instance != this)
        {
            instance.setMsg();
            instance.updateLevelLines();
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            SetUp();
        }
        //lines[0].debugShout();

    }


    private void SetUp()
    {
        // Set up level object structure.
        // TODO, make more automatic
        //FindWithTag("LevelLine") 
        levelDones = new bool[levelList.Length];
        levelsCollected = new bool[levelList.Length];
        string [] levelLineNames = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};
        lines = new LevelLine[levelLineNames.Length];
        msg = GameObject.Find("CollectibleNotify");
        updateLevelLines();
        


    }

    // Passing data down, updating panal.

    private void updateLevelLines()
    {
        // Updates GameState references to refer to the current scenes objects. And then passes down data to those objects
        
        msg.SetActive(false);

        //Gives level lines the correct panel ref, sets each level line to value from varaibles levelDones
        string [] levelLineNames = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};
        int startIndex = 0;
        for (int i = 0; i < levelLineNames.Length; i++)
        {
            lines[i] = GameObject.Find(levelLineNames[i]).GetComponent<LevelLine>();
            lines[i].setCompletion(levelDones, levelsCollected, startIndex);
            startIndex += lines[i].levelCount;
        }

        thisLevelCollectibles =
            GameObject.FindGameObjectsWithTag("Collectible").Length;
    }

    // Updating attributes

    public void EndLevel()
    {
        Debug.Log("We ending");
        int levelNum =
            Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
        if (thisLevelCollectibles == 0)
        {            
            levelsCollected[levelNum] = true;
        }
        levelDones[levelNum] = true;
        updateLevelLines();

    }

    public void Collected()
    {
        thisLevelCollectibles--;
        if (thisLevelCollectibles == 0)
        {
            msg.SetActive(true);
        }
    }
    
    public static int LevelCount ()
    {
        return levelList.Length;
    }

    
    public void markDone(int levelNum)
    {
        levelDones[levelNum] = true;
    }

    public void markComplete(int levelNum)
    {
        levelsCollected[levelNum] = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void setMsg()
    {
        msg = GameObject.Find("CollectibleNotify");
    }

    
}
