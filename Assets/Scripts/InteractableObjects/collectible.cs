using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collectible : MonoBehaviour
{
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
            Debug.Log("Fetcged!");
            var tracker = GameObject.Find("CollectibleTracker");
            tracker.GetComponent<CollectibleTracker>().Collected();
            Destroy(gameObject);
        }
    }
}
