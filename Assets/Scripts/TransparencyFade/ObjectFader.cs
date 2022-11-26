using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// ObjectFader
//-----------------------------------------//
// Handles making an object transparent or opaque as needed. According to whether object is between player and camera. 

public class ObjectFader : MonoBehaviour
{
    FindObjsToFade hitList;
    public bool faded;

    // Find Camera's list of collisions hit
    void Awake()
    { 
        hitList = GameObject.FindWithTag("MainCamera").GetComponent<FindObjsToFade>();
        faded = false;
    }

    // Only called once per frame, if and only if Camera says this needs to be faded.
    public void Fade()
    { 
        faded = true;

        Renderer objectRenderer = GetComponent<Renderer>();

        foreach (Material material in objectRenderer.materials)
        {
            MaterialObjectFade.MakeFade(material);

            Color newColor = material.color;
            newColor.a = 0.3f;
            material.color = newColor;
        }
    }

    // So long as we are faded, keep checking if we are still inside the list to be faded, if not, unfade.
    void Update()
    {
        if (faded)
        {
            bool stayFaded = false;
            int i = 0;
            int max = hitList.numOfHits;

            foreach (RaycastHit hit in hitList.hits)
            {

                if (i > max){
                    break;
                }
                
                if (transform == hit.transform)
                {
                    stayFaded = true;
                }
                
                
                if (hit.collider != null){
                    i++;
                }
            }
            
            faded = stayFaded;
            if (!stayFaded){
                Unfade();
            }
        }
    }

    private void Unfade()
    {
        Renderer objectRenderer = GetComponent<Renderer>();
        foreach (Material material in objectRenderer.materials)
        {
            MaterialObjectFade.MakeOpaque(material);
   
            Color newColor = material.color;
            newColor.a = 1f;
            material.color = newColor;
        }
    }

    public bool IsFade()
    {
        return faded;
    }
}