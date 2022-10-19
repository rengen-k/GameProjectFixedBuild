using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
public class CollectibleTracker : MonoBehaviour
{
    // Start is called before the first frame update

    private static bool[] levelsCollected;
    public static CollectibleTracker instance;
    [SerializeField] private Transform levelPanel;

    void Awake()
    {
        
        if (instance != null && instance != this){
            Debug.Log("Collectible FOund!");
            Destroy(this.gameObject);
        }
        else{
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            Debug.Log("NoCollectible FOund!");
            //Generating list, with every level represented by a bool in levelsCollected
            GenerateLevelList();
            
        }
        UpdateLevels();

    }

    void Start()
    {
        


    }

    private void GenerateLevelList()
    {
        levelsCollected = new bool[levelPanel.childCount];
        //TESTING
        levelsCollected[6] = true;
    }

    // UpdateLevels gets called during object creation, and whenever a collectible is picked up, after the levelsCollected gets updated.
    public void UpdateLevels()
    {
        for (int i = 0; i < levelsCollected.Length; i++){
            if (levelsCollected[i])
            {
                //bool says this level has collected the collectible. Now update correct buttn in panel
                //i is the level number you got to update, levelNum is the active scenes number, dunno what to use it for
                int levelNum = Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
                Debug.Log("Got " + levelNum);


            }
        }
    }
}
