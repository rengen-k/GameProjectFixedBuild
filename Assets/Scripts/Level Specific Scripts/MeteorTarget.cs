using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeteorTarget : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("Time until the meteor lands")]
    [SerializeField] private float delay = 5f;
    [Tooltip("References prefab that will spawn to simulate meteor")]
    [SerializeField] private GameObject meteor;
    private float countdown;
    private Transform timer;
    private Vector3 startScaleSize;
    void Awake()
    {
        timer = transform.Find("WorldSpaceCanvas/timer");
        timer.transform.parent.GetComponent<Canvas>().worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        countdown = delay;
        startScaleSize = timer.GetComponent<RectTransform>().localScale;
    }

    public void SetDelay(float d)
    {
        delay = d;
        countdown = d;
    }

    void Start()
    {
        // Countdown reticle counts down, when done, spawn object high above that crashes down at random.
    }

    // Update is called once per frame
    void Update()
    {
        Camera camera = Camera.main;
        timer.transform.LookAt(transform.position + camera.transform.rotation * Vector3.forward, camera.transform.rotation * Vector3.up);
        
        countdown -= Time.deltaTime;
        timer.GetComponent<Image>().fillAmount -= 1.0f / delay * Time.deltaTime;
        if (countdown <= 2)
        {
            timer.GetComponent<RectTransform>().localScale = Vector3.Lerp( new Vector3(0.2f, 0.2f, 0.2f), startScaleSize, countdown/2);
            timer.GetComponent<Image>().color = new Color(1, 0, 0, 0.8f);
        }
        if (countdown <= 0)
        {
            Explode();
        }
    }

    private void Explode()

    {
        Vector3 pos = transform.position;
        pos += new Vector3(Random.Range(-10, 10), 60, Random.Range(-10, 11));

        Instantiate(meteor, pos, transform.rotation).GetComponent<Meteor>().SetUp( GetComponent<Transform>().localScale.x, transform.position);
        enabled = false;
        StartCoroutine(Wait());
    }
    

    // Credit Programmer
    // https://stackoverflow.com/a/46587297
    // Code to increase object scale over time. Modified slightly.
    // Make sure there is only one instance of this function running
    IEnumerator scaleOverTime(Transform objectToScale, Vector3 toScale, float duration)
    {
       
        float counter = 0;

        //Get the current scale of the object to be moved
        Vector3 startScaleSize = objectToScale.localScale;

        while (counter < duration)
        {
            counter += Time.deltaTime;
            objectToScale.localScale = Vector3.Lerp(startScaleSize, toScale, counter / duration);
            yield return null;
        }

        Object.Destroy(gameObject);
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(2);
        Object.Destroy(gameObject);
    }
}
