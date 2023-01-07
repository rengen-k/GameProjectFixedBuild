using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadLevel : MonoBehaviour
{
    private SceneSwitch scene;

    void Start()
    {
        scene = GameObject.FindGameObjectWithTag("LevelLoader").GetComponent<SceneSwitch>();
    }

    public void Load()
    {
        scene.LoadScene();
    }
}
