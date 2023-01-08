using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckIfSoundEffectsPresent : MonoBehaviour
{
    private bool end = false;

    void Update()
    {
        if(!end)
        {
            if(GameObject.FindGameObjectsWithTag("LevelSoundEffects")[0].active && GameObject.FindWithTag("SoundEffects") != null || GameObject.FindGameObjectsWithTag("LevelSoundEffects").Length == 2)
            {
                Destroy(this.gameObject);
                end = true;
            } else {
                end = true;
            }
        }
    }
}
