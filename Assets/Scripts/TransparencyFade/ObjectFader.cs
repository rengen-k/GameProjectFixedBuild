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
        faded = true;

        Renderer objectRenderer = GetComponent<Renderer>();

        foreach (Material material in objectRenderer.materials)
        {
            material.SetOverrideTag("RenderType", "Transparent");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            material.SetInt("_ZWrite", 0);
            material.DisableKeyword("_ALPHATEST_ON");
            material.EnableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;

            Color newColor = material.color;
            newColor.a = 0.3f;
            material.color = newColor;
        }
    }

    void Update ()
    {
        // So long as we are faded, keep checking if we are still inside the list to be faded, if not, unfade. 
        if (faded)
        {
            bool unfadeCheck = false;
            int i = 0;
            int max = hitList.numOfHits;
            foreach (RaycastHit hit in hitList.hits)
            {
                if (i < max){
                    break;
                }
                if (transform == hit.transform)
                {
                    unfadeCheck = true;
                }
                i++;
            }
            faded = unfadeCheck;
            if (!unfadeCheck){
                Unfade();
            }
        }

        
    }

    private void Unfade()
    {

        Renderer objectRenderer = GetComponent<Renderer>();
        foreach (Material material in objectRenderer.materials)
        {
            
            material.SetOverrideTag("RenderType", "");
            material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
            material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.Zero);
            material.SetInt("_ZWrite", 1);
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.renderQueue = -1;
   
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