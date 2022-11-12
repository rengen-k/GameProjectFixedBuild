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

    // Set range to (0, X+1) where X is the number of levels
    public int [] levelList = Enumerable.Range(0, 10).ToArray();
    private GameObject levelPanel;
    private LevelLine [] lines;
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

        lines[0] = new LevelLine("Player");
    }

    private void updateLevelLines()
    {
        //Gives level lines the correct panel ref
        string [] levelLineNames = {"TutorialLine", "SimpleLine", "BlockLine", "MetaLine"};
        int i = 0;
        foreach (LevelLine l in lines)
        {
            l.SetRef(levelLineNames[i]);
            i++;
        }
    }

    void Start()
    {
        
        lines[0].debugShout();

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
