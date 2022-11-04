using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VagueText : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] float timeToAppear = -1f;
    void Start()
    {
        if (timeToAppear < 0)
        {
            timeToAppear = 10f;
        }
        GetComponent<Renderer>().enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        timeToAppear -= Time.deltaTime;
        if (timeToAppear < 0)
        {
            GetComponent<Renderer>().enabled = true;
        }
    }
}
