using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
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
        easy,
        normal,
        hard
    }

    [SerializeField] private Difficulty diff;
    
    //Level completion management
    // Set range to (0, X+1) where X is the number of levels
    private static int [] levelList = Enumerable.Range(0, 21).ToArray();
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
    public int thisLevelCollectibles = 0;
    
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
            if (SceneManager.GetActiveScene().name != "DemoStart")
            {
                ResetSingleton();
                UpdateLevelLines();
            }
            
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
        int i = 0;
        string [] names = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};
        foreach (string s in names)
        {
            lines[i] = GameObject.Find(s).GetComponent<LevelLine>();
            i++;
        }
        
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
        lines[0].sets[0].ForceOpen();
        lines[1].sets[0].ForceOpen();
        //handles the bonus levels opening.
        
        
        
        for (int i = 0; i < lines.Length; i++)
        {
            lines[i].setCompletion(levelDones, levelsCollected, startIndex);
            startIndex += lines[i].levelCount;
            //README, Set this if statement such that it only iterates through the lines that if done, open next set.
            if (lines[i].isDone() && i <= 1)
            {
                lines[i+1].openFirstSet();
            }
        }

        if (totalCollectibles >= 5 || lines[0].isDone())
        {
            lines[3].sets[0].ForceOpen();
        }
        if (totalCollectibles >= 10 || lines[1].isDone())
        {
            lines[3].sets[1].ForceOpen();
        }
        if (totalCollectibles >= 15 || lines[2].isDone())
        {
            lines[3].sets[2].ForceOpen();
        }
        lines[3].setCompletion(levelDones, levelsCollected, 18);

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

        SaveGame();


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

    public bool isNormal()
    {
        return diff == Difficulty.normal || diff == Difficulty.easy;
    }

    public bool IsEasy()
    {
        return diff == Difficulty.easy;
    }

    public int IncrementDifficulty()
    {
        int length = Enum.GetNames(typeof(Difficulty)).Length;
        int index = ((int) ++diff) % (length);
        diff = (Difficulty) index;
        return index;

    }

    public int GetDifficulty()
    {
        return (int) diff;
    }

    public void SaveGame()
    {
        BinaryFormatter bf = new BinaryFormatter(); 
        FileStream file = File.Create(Application.persistentDataPath 
                    + "/Save.dat"); 
        SaveData data = new SaveData();
        data.levelDones = levelDones;
        data.levelsCollected = levelsCollected;
        data.totalCollectibles = totalCollectibles;
        bf.Serialize(file, data);
        file.Close();
        Debug.Log("Game saved!");
    }

    public void LoadGame()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.dat"))
	    {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = 
                    File.Open(Application.persistentDataPath 
                    + "/Save.dat", FileMode.Open);
            SaveData data = (SaveData)bf.Deserialize(file);
            file.Close();
            levelDones = data.levelDones;
            levelsCollected = data.levelsCollected;
            totalCollectibles = data.totalCollectibles;
            Debug.Log("Game loaded!");
        }
        else
            Debug.LogError("There is no save data!");
        }

    public void DeleteSave()
    {
        if (File.Exists(Application.persistentDataPath + "/Save.dat"))
        {
            File.Delete(Application.persistentDataPath + "/Save.dat");
        }
        else
        {
            Debug.Log("No save detected");
        }
    }
    
}

// Saved Data
[Serializable]
class SaveData
{
    public bool [] levelDones;
    public bool [] levelsCollected;
    public int totalCollectibles = 0;
}
