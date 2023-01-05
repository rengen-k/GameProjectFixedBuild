using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BossEnemy : MonoBehaviour
{
    public GameObject ground;
    public GameObject enemy;
    public Vector3 newPos;
    public int health = 100;
    public Transform target;
    public float speed = 4f;
    public GameObject attackRing;
    public GameObject attackPosition;
    private float _elapsedTime;
    private float _timeToWaypoint;
    [SerializeField] private float _speed = 5;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarGameObj;
    private int count;
    Rigidbody rig;
    float magnitude = 15f;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        target = GameObject.Find("Player").transform;
        StartCoroutine(SpeedLoop());
        healthBar = healthBar.GetComponent<Image>();
    }

    void Update()
    {
        if(health <= 0)
        {
            Destroy(gameObject);
            Destroy(healthBarGameObj);

            //Instantiate()
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
            healthBar.fillAmount -= 0.1f;
        }
    }

    IEnumerator SpeedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            speed = 10;
            yield return new WaitForSeconds(8f);
            speed = 4f;
        }
    }
}