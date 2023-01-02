using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossEnemy : MonoBehaviour
{
    public GameObject ground;
    public GameObject enemy;
    public Vector3 newPos;
    public int health = 250;
    public Transform target;
    public float speed = 4f;
    public GameObject attackRing;
    public GameObject attackPosition;
    private float _elapsedTime;
    private float _timeToWaypoint;
    [SerializeField] private float _speed = 5;
    private int count;
    Rigidbody rig;
    float magnitude = 15f;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        target = GameObject.Find("Player").transform;
        StartCoroutine(SpeedLoop());
    }

    void Update()
    {
        print(health);
        if(health <= 0)
        {
            Destroy(gameObject);
        }

    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        rig.MovePosition(pos);
        //transform.LookAt(target);
    }

    private void OnTriggerEnter(Collider col)
    {
        print(col);
        if(col.GetComponent<Collider>().tag == "HurtTag1")
        {
            health -= 10;
            print(health);
        }
    }

    IEnumerator SpeedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(10f);
            speed = 8f;
            yield return new WaitForSeconds(8f);
            speed = 4f;
        }
    }
}