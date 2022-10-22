using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableObject : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (transform.parent != null) {
            other.transform.SetParent(transform);
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        other.transform.SetParent(null);
    }
}
