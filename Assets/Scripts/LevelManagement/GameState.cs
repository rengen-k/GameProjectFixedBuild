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
// merge LevelSlector into Level, tie scripts to structure moer cleanly,
// CollectibleTracker in here.
// GameState finds refs to levels inside sets- passes done and collected.

public class GameState : MonoBehaviour
{
    public static GameState instance;

    //Level completion management
    // Set range to (0, X+1) where X is the number of levels
    private static int [] levelList = Enumerable.Range(0, 10).ToArray();
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
        if (instance != null && instance != this)
        {
            instance.updateLevelLines();
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            SetUp();
        }
    }

    private void SetUp()
    {
        // Set up level object structure.
        // TODO, make more automatic
        //FindWithTag("LevelLine") 
        string [] levelLineNames = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};
        lines = new LevelLine[levelLineNames.Length];
        levelDones = new bool[levelLineNames.Length];
        levelsCollected = new bool[levelLineNames.Length];


    }

    private void updateLevelLines()
    {
        //Gives level lines the correct panel ref, sets each level line to value from varaibles levelDones
        string [] levelLineNames = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};
        int i = 0;
        int startIndex = 0;
        foreach (LevelLine l in lines)
        {
            l.SetRef(levelLineNames[i]);
            //l.setCompletion(levelDones, levelsCollected, startIndex);
            i++;
            startIndex += l.levelCount;
        }
    }
    
    public static int LevelCount ()
    {
        return levelList.Length;
    }

    void Start()
    {
        
        lines[0].debugShout();

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
}
