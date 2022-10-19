using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleTracker : MonoBehaviour
{
    // Start is called before the first frame update

    private static bool[] levelsCollected;
    public static CollectibleTracker instance;

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
        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
