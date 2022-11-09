using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// VagueText
//-----------------------------------------//
// Creates text to appear after a certain duration

public class VagueText : MonoBehaviour
{
    [Tooltip("How long until this text should appear.")]
    [SerializeField] float timeToAppear = -1f;
    void Start()
    {
        if (timeToAppear < 0)
        {
            timeToAppear = 10f;
        }
        GetComponent<Renderer>().enabled = false;
    }

    void Update()
    {
        timeToAppear -= Time.deltaTime;
        if (timeToAppear < 0)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
}
