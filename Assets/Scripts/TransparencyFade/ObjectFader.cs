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
        // So long as we are faded, keep checking if we are still inside the list to be faded, if not, unfade. 
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
                Unfade();
            }
        }

        
    }

    private void Unfade()
    {
        Debug.Log("Unfading");
    }

    public bool IsFade()
    {
        return faded;
    }

}