using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfMusicPresent : MonoBehaviour
{
    private bool end = false;

    void Update()
    {
        if(!end)
        {
            if(GameObject.FindGameObjectsWithTag("LevelMusic")[0].active && GameObject.FindWithTag("Music") != null || GameObject.FindGameObjectsWithTag("LevelMusic").Length == 2)
            {
                Destroy(this.gameObject);
                end = true;
            } else {
                end = true;
            }
        }
    }
}
