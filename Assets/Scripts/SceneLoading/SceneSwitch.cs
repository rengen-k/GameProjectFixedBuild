using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneSwitch : MonoBehaviour
{
    [SerializeField] private string nameOfSceneToLoad;
    [SerializeField] private int indexOfSceneToLoad;
    [SerializeField] private bool loadWithIndex;
    
    private void OnTriggerEnter(Collider collision) {
        GameObject collisionGameObject = collision.gameObject;
        if (collisionGameObject.tag == "Player") {
            var tracker = GameObject.Find("CollectibleTracker");
            tracker.GetComponent<CollectibleTracker>().EndLevel();
            LoadScene();
        }
    }

    private void LoadScene() {
        SceneManager.LoadScene(nameOfSceneToLoad);
    }
}
