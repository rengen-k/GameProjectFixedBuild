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
    [SerializeField] private int indexOfSceneToLoad;
    [SerializeField] private bool loadWithIndex;

    [SerializeField] private Animator fade;

    void Start()
    {
        fade = GameObject.FindGameObjectWithTag("Fade").GetComponent<Animator>();
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
            fade.SetTrigger("FadeOut");
            //LoadScene();
        }
    }

    public void LoadScene() {
        SceneManager.LoadScene(nameOfSceneToLoad);
    }

}