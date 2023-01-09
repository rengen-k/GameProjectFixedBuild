using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trampoline_Parent : MonoBehaviour
{
    private GameObject redDoor;

    void Awake()
    {
        redDoor = GameObject.Find("KeyDoor (3)");
    }

    // Update is called once per frame
    void Update()
    {
        if (!redDoor.activeInHierarchy) {
            transform.SetParent(null);
        }
    }
}
