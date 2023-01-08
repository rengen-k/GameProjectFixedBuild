using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfCurrentSettingsPresent : MonoBehaviour
{
    private bool end = false;

    void Update()
    {
        if(!end)
        {
            if(GameObject.FindGameObjectsWithTag("LevelCurrentSettings")[0].active && GameObject.FindWithTag("CurrentSettings") != null || GameObject.FindGameObjectsWithTag("LevelCurrentSettings").Length == 2)
            {
                Destroy(this.gameObject);
                end = true;
            } else {
                end = true;
            }
        }
    }
}