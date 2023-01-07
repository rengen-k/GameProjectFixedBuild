using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    //Level Loader script, to advance to next level
    [Tooltip("Fields deciding what the next level to load is.")]
    [SerializeField] private string nameOfSceneToLoad;
    [SerializeField] private int indexOfSceneToLoad;
    [SerializeField] private bool loadWithIndex;

    public AudioSource soundManager;
    public AudioClip teleport;

    private void Start()
    {
        soundManager = soundManager.GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider collision) {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.tag == "Player") {
            soundManager.PlayOneShot(teleport);
            var tracker = GameObject.Find("CollectibleTracker");
            if (tracker == null)
            {
                tracker = GameObject.Find("GlobalGameState");
            }
            tracker.GetComponent<GameState>().EndLevel();
            LoadScene();
        }
    }

    private void LoadScene() {
        SceneManager.LoadScene(nameOfSceneToLoad);
    }
}
