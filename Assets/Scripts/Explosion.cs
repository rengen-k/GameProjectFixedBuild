using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    // Start is called before the first frame update
    public float bombRadious;
    private float radious;
    private bool isScaling = false;

    void Start()
    {
        StartCoroutine(scaleOverTime(transform, new Vector3(bombRadious, bombRadious, bombRadious), 0.2f));
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
        //Make sure there is only one instance of this function running
        if (isScaling)
        {
            yield break; ///exit if this is still running
        }
        isScaling = true;

        float counter = 0;

        //Get the current scale of the object to be moved
        Vector3 startScaleSize = objectToScale.localScale;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, toScale, counter / duration);
            yield return null;
        }

        isScaling = false;
        yield return new WaitForSeconds(0.25f);
        Object.Destroy(gameObject);
    }
}
