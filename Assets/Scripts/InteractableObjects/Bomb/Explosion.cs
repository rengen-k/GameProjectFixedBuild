using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//-----------------------------------------//
// Explosion
//-----------------------------------------//
// Attached to sphere to make sphere prefab simulate explosion.

[RequireComponent(typeof(AudioSource))]
public class Explosion : MonoBehaviour
{
    public float bombRadious;
    private float radious;
    private bool isScaling = false;
    [Tooltip("How long will it take for the explosion to reach maximum radious.")]
    [SerializeField] float timeToExpand;
    [Tooltip("How long explosion will stay are maximum radious.")]
    [SerializeField] float timeToStayFull;
    [Tooltip("The explosion clip played")]
    [SerializeField] private AudioClip clip;

    void Start()
    {
        StartCoroutine(scaleOverTime(transform, new Vector3(bombRadious, bombRadious, bombRadious), timeToExpand));

        AudioSource.PlayClipAtPoint(clip, GameObject.Find("Main Camera").transform.position);
    }

    // Credit Programmer
    // https://stackoverflow.com/a/46587297
    // Code to increase object scale over time. Modified slightly.
    // Make sure there is only one instance of this function running
    IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
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
        yield return new WaitForSeconds(timeToStayFull);
        Object.Destroy(gameObject);
    }
}
