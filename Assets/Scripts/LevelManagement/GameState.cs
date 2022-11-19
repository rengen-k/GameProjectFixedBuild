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

public class GameState : MonoBehaviour
{
    public static GameState instance;

    public enum Difficulty
    {
        normal,
        hard
    }

    // Data that needs to be saved

    [SerializeField] private Difficulty diff;

    //Level completion management
    // Set range to (0, X+1) where X is the number of levels
    private static int [] levelList = Enumerable.Range(0, 11).ToArray();
    private bool [] levelDones;
    // Reference Array to each LevelLine Object, automatically generated.

    // Data that is generated as needed.
    private LevelLine [] lines;

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
        diff = Difficulty.normal;
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
        GameObject [] linesObj = GameObject.FindGameObjectsWithTag("LevelLine");
        lines = linesObj.Select( line => line.GetComponent<LevelLine>()).ToArray();
        
        msg = GameObject.Find("CollectibleNotify");
        thisLevelCollectibles = 0;


    }

        
    //Level Logic

    private void UpdateLevelLines()
    {
        // Updates GameState references to refer to the current scenes objects. And then passes down data from this to those objects
        
        msg.SetActive(false);

        //Gives level lines the correct panel ref, sets each level line to value from varaibles levelDones
        int startIndex = 0;
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].setCompletion(levelDones, levelsCollected, startIndex);
            startIndex += lines[i].levelCount;
        }

        thisLevelTotalCollectibles =
            GameObject.FindGameObjectsWithTag("Collectible").Length;
    }


    public void EndLevel()
    {
        // When a level ends, time to update singleton to match.
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

    // Difficulty

    public bool IsNormal()
    {
        return diff == Difficulty.normal;
    }

    public int IncrementDifficulty()
    {
        int length = Enum.GetNames(typeof(Difficulty)).Length;
        int index = ((int) ++diff) % (length);
        diff = (Difficulty) index;
        Debug.Log(index + " " + diff);
        return index;

    }


    
}
