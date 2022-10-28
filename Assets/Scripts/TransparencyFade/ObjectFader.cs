using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFader : MonoBehaviour
{

    FindObjsToFade hitList;
    bool faded;
    void Awake()
    {
        // Find Camera's list of collisions hit, 
        hitList = GameObject.FindWithTag("MainCamera").GetComponent<FindObjsToFade>();
        faded = false;


    }

    public void Fade()
    {
        Debug.Log(transform.name + " should be faded");
        faded = true;
    }

    void Update ()
    {
        if (faded)
        {
            bool unfadeCheck = false;
            foreach (RaycastHit hit in hitList.hits)
            {
                if (transform == hit.transform)
                {
                    unfadeCheck = true;
                }
            }
            faded = unfadeCheck;
            if (!unfadeCheck){
            Debug.Log(transform.name + " should unfade");
            }
        }
        
    }

    public bool IsFade()
    {
        return faded;
    }

}