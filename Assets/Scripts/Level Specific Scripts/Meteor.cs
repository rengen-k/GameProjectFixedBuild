using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class Meteor : MonoBehaviour
{
    // Start is called before the first frame update
    [Tooltip("References prefab that will spawn to simulate explosion")]
    [SerializeField] private GameObject explosion;
    public float radious = 5;
    private float timeDuration = 2f;
    private float timeTaken = 0;
    private Vector3 target;
    private Vector3 start;
    [Tooltip("The explosion clip played when the meteor impacts")]
    [SerializeField] private AudioClip clip;
    public void SetUp(float rad, Vector3 goTo)
    {
        start = transform.position;
        radious = rad;
        target = goTo;
        transform.LookAt(target);
    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.LerpUnclamped(start, target, timeTaken / timeDuration);
        timeTaken += Time.deltaTime;

    }

    void OnCollisionEnter(Collision other)
    {

        Impact();
        GameObject.Destroy(gameObject);
    }

    void Impact()
    {
        AudioSource.PlayClipAtPoint(clip, GameObject.Find("Main Camera").transform.position);
        Instantiate(explosion, transform.position, transform.rotation).GetComponent<Explosion>().bombRadious = radious+3;
    }


}
