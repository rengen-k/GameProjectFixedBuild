using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRangeTrigger : MonoBehaviour
{
    private bool playerInRange = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = true;
        }
    }

    private void OnCollisionExit(Collision other) {
        if (other.gameObject.tag == "Player")
        {
            playerInRange = false;
        }
    }
}
