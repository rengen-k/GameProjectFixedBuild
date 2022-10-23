using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private Transform previousParent = null;
    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null && other.gameObject.tag != "Ground" && this.transform.parent.tag == "MovingPlatform") {
            other.transform.SetParent(transform);
            previousParent = other.transform.parent;
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (previousParent == null) {
            other.transform.SetParent(null);
        } else {
            other.transform.SetParent(previousParent);
        }
        
    }
}
