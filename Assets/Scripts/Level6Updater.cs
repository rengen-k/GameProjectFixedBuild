using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level6Updater : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

        GetComponent<CollectibleTracker>().EndLevel();
    }

}
