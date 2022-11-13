using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
    //Collectible, once touched, destroys itself and tells singleton that in this scene, 1 collectible was collected.

    //To ensure that OnTrggerEnter can never trigger more then once.
    private bool onceTouch = true;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {   
        if (other.gameObject.tag == "Player" && onceTouch){
            onceTouch = false;
            var tracker = GameObject.Find("CollectibleTracker");
            if (tracker == null)
            {
                tracker = GameObject.Find("GlobalGameState");
            }
            tracker.GetComponent<GameState>().Collected();
            Destroy(gameObject);
        }
    }
}
