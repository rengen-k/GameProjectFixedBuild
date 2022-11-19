using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class MeteorTarget : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("Time until the meteor lands")]
    private float delay = 5f;
    [Tooltip("References prefab that will spawn to simulate meteor")]
    [SerializeField] private GameObject meteor;
    [SerializeField] private float countdown;
    private Transform timer;

    void Awake()
    {
        timer = transform.Find("WorldSpaceCanvas/timer");
        timer.transform.parent.GetComponent<Canvas>().worldCamera = GameObject.Find("UICamera").GetComponent<Camera>();
        countdown = delay;
    }

    public void setDelay(float d)
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
            timer.GetComponent<RectTransform>().localScale += new Vector3(0.06f, 0.06f, 0.06f);
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
        pos += new Vector3(Random.Range(-10, 11), 30, Random.Range(-10, 11));

        Instantiate(meteor, pos, transform.rotation).GetComponent<Meteor>().SetUp( GetComponent<Transform>().localScale.x, transform.position);
        Object.Destroy(gameObject);
    }
}
