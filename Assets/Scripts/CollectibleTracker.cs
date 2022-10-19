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
    [SerializeField] private Transform levelPanel;

    void Awake()
    {
        //If an object from another scene like this exists, use that, otherwise, set up instance to be the object from another scene upon leaving scene.
        if (instance != null && instance != this){
            Destroy(this.gameObject);
        }
        else{
            DontDestroyOnLoad(transform.gameObject);
            instance = this;
            //Generating list, with every level represented by a bool in levelsCollected
            levelsCollected = new bool[levelPanel.childCount];
            
            
        }
        UpdateLevels();

    }

    void Start()
    {
        


    }

    public void Collected()
    {
        //get scene number, take as index of level we should update in list.
        int levelNum = Int32.Parse(SceneManager.GetActiveScene().name.Split(" ")[1]);
        levelsCollected[levelNum] = true;
        UpdateLevels();
    }

    // UpdateLevels gets called during object creation, and whenever a collectible is picked up, after the levelsCollected gets updated.
    private void UpdateLevels()
    {
        int index = 0;
        foreach (Transform btnObj in levelPanel)
        {
            if (levelsCollected[index])
            {
                
                btnObj.gameObject.GetComponent<Image>().color = Color.green;
            }
            
            index++;
        }


    }
}
