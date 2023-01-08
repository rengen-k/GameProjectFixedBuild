using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;

public class SceneSwitch : MonoBehaviour
{
    //Level Loader script, to advance to next level
    [Tooltip("Fields deciding what the next level to load is.")]
    [SerializeField] private string nameOfSceneToLoad;
    [SerializeField] private bool autoLoad;
    private Scene currentScene;
    private string currentSceneName;
    private string nextSceneName;
    private GameState gameState;

    [SerializeField] private Animator fade;

    void Start() {
        gameState = GameObject.Find("GlobalGameState").GetComponent<GameState>();
        currentScene = SceneManager.GetActiveScene();
        currentSceneName = currentScene.name;
        string[] words = currentSceneName.Split(' ');
        nextSceneName = words[0] + " " + (Int32.Parse(words[1]) + 1);
        soundManager = GameObject.Find("SoundManager").GetComponent<AudioSource>();

    void Update()
    {
        fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
        print("1");
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

            if (currentSceneName == "Level 15" && gameState.totalCollectibles >= 25)
            {
                 SceneManager.LoadScene("True_Ending");
            }

            if (autoLoad)
            {
                SceneManager.LoadScene(nextSceneName);
            }

            LoadScene();
            fade.SetTrigger("FadeOut");
            //LoadScene();
        }
    }

    public void LoadScene() {
        SceneManager.LoadScene(nameOfSceneToLoad);
    }

    public void LoadLevel0() {
        SceneManager.LoadScene("Level 0");
    }
}
}
