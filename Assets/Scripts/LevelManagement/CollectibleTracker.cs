using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//-----------------------------------------//
// CollectibleTracker
//-----------------------------------------//
// Singleton that keeps track of which levels has had all of their collectibles collectied. Updates the internal representation of the level when EndLevel is called, normally when levelloader is touched.
public class CollectibleTracker : MonoBehaviour
{
    private static bool[] levelsCollected;

    public static CollectibleTracker instance;

    // Needs panel inside LevelScreen from prefab, should be one with the grid of buttons.
    private GameObject levelPanel;

    private GameObject msg;

    private int thisLevelCollectibles;

    void Awake()
    {
        //If an object from another scene like this exists, use that, otherwise, set up instance to be the object from another scene upon leaving scene.
        if (instance != null && instance != this)
        {
            instance.UpdateGlobalInstance();
            Destroy(this.gameObject);
        }
        else
        {
            DontDestroyOnLoad(transform.gameObject);
            instance = this;

            //Generating list, with every level represented by a bool in levelsCollected
            levelPanel = GameObject.Find("LevelGrid");
            msg = GameObject.Find("CollectibleNotify");
            levelsCollected = new bool[levelPanel.transform.childCount];
            instance.UpdateGlobalInstance();
        }

    }

    public void EndLevel()
    {
        // End level reached, update singleton if all collectibles of this level was attained.

        if (thisLevelCollectibles == 0)
        {
            int levelNum =
                Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
            levelsCollected[levelNum] = true;
        }
        UpdateLevels();
    }

    public void Collected()
    {
        thisLevelCollectibles--;
        if (thisLevelCollectibles == 0)
        {
            msg.SetActive(true);
        }
    }

    // UpdateLevels gets called during object creation, and whenever a collectible is picked up, after the levelsCollected gets updated.
    private void UpdateLevels()
    {
        // Using singleton, update level panel in pause menu to indicate every level that has been 100% completed; all collectibles grabbed in one run, at some point.
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
        // Update singleton just loaded from another scene.
        levelPanel = GameObject.Find("LevelGrid");
        thisLevelCollectibles =
            GameObject.FindGameObjectsWithTag("Collectible").Length;
        msg = GameObject.Find("CollectibleNotify");
        msg.SetActive(false);

        UpdateLevels();
    }
}
