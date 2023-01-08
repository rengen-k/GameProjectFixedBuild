using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    //Level Loader script to advance to next level
    [Tooltip("Fields deciding what the next level to load is.")]
    [SerializeField] private string nameOfSceneToLoad;
    [SerializeField] private bool autoLoad;
    private Scene currentScene;
    private string currentSceneName;
    private string nextSceneName;


    private void Start() {
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        string[] words = currentSceneName.Split(' ');
        nextSceneName = words[0] + " " + (Int32.Parse(words[1]) + 1);
    }
    
    private void OnTriggerEnter(Collider collision) {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.tag == "Player") {
            var tracker = GameObject.Find("CollectibleTracker");
            if (tracker == null)
            {
                tracker = GameObject.Find("GlobalGameState");
            }
            tracker.GetComponent<GameState>().EndLevel();
            if (autoLoad)
            {
                SceneManager.LoadScene(nextSceneName);
            }
            LoadScene();
        }
    }

    private void LoadScene() {
        SceneManager.LoadScene(nameOfSceneToLoad);
    }
}
