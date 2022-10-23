using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class CollectibleTracker : MonoBehaviour
{
    // Start is called before the first frame update

    private static bool[] levelsCollected;
    public static CollectibleTracker instance;
    //Needs panel inside LevelScreen from prefab, should be one with the grid of buttons.
    [SerializeField] private GameObject levelPanel;
    private int thisLevelCollectibles;

    void Awake()
    {
        //If an object from another scene like this exists, use that, otherwise, set up instance to be the object from another scene upon leaving scene.
        if (instance != null && instance != this){
            instance.UpdateGlobalInstance();
            Destroy(this.gameObject);
        }
        else{
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            //Generating list, with every level represented by a bool in levelsCollected
            levelPanel = GameObject.Find("LevelGrid");
            levelsCollected = new bool[levelPanel.transform.childCount];
            
            
        }
        levelPanel = GameObject.Find("LevelGrid");
        thisLevelCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;
        UpdateLevels();

    }

    void Start()
    {
        


    }

    public void EndLevel()
    {
        if (thisLevelCollectibles == 0){
            int levelNum = Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
            levelsCollected[levelNum] = true;
        }
        
        UpdateLevels();
    }

    public void Collected()
    {

        thisLevelCollectibles--;
        
    }

    // UpdateLevels gets called during object creation, and whenever a collectible is picked up, after the levelsCollected gets updated.
    private void UpdateLevels()
    {
        int index = 0;
        foreach (Transform btnObj in levelPanel.transform)
        {
            if (levelsCollected[index])
            {
                
                btnObj.gameObject.GetComponent<Image>().color = Color.green;
            }
            
            index++;
        }


    }

    public void UpdateGlobalInstance()
    {
        levelPanel = GameObject.Find("LevelGrid");
        thisLevelCollectibles = GameObject.FindGameObjectsWithTag("Collectible").Length;

        UpdateLevels();
    }
}
