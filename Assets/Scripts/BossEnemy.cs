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
    public int health;
    public Transform target;
    private float speed;
    private float newSpeed;
    private float baseSpeed;
    public GameObject attackRing;
    public GameObject attackPosition;
    private float _elapsedTime;
    private float _timeToWaypoint;
    private float healthFillAmount;
    [SerializeField] private Image healthBar;
    [SerializeField] private GameObject healthBarGameObj;
    [SerializeField] private GameState globalGameState;
    [SerializeField] private GameObject loadLevel;
    private int count;
    Rigidbody rig;

    void Start()
    {
        rig = GetComponent<Rigidbody>();
        target = GameObject.Find("Player").transform;
        StartCoroutine(SpeedLoop());
        healthBar = healthBar.GetComponent<Image>();
        globalGameState = GameObject.Find("GlobalGameState").GetComponent<GameState>();


        if (globalGameState.GetDifficulty() == 0)
        {
            Debug.Log("easy");
            health = 30;
            baseSpeed = 2f;
            newSpeed = 6f;
            healthFillAmount = 0.3333333333333334f;
        } 
        else if (globalGameState.GetDifficulty() == 1)
        {
            Debug.Log("normal");
            health = 60;
            baseSpeed = 3f;
            newSpeed = 8f;
            healthFillAmount = 0.1666666666666667f;
        } 
        else if (globalGameState.GetDifficulty() == 2)
        {
            Debug.Log("hard");
            health = 100;
            baseSpeed = 4f;
            newSpeed = 10f;
            healthFillAmount = 0.1f;
        }

        speed = baseSpeed;

    }

    void Update()
    {
        globalGameState = GameObject.Find("GlobalGameState").GetComponent<GameState>();
        if(health <= 0)
        {
            Instantiate(loadLevel, gameObject.transform.position, gameObject.transform.rotation);
            Destroy(gameObject);
            Destroy(healthBarGameObj);
        }

    }

    void FixedUpdate()
    {
        Vector3 pos = Vector3.MoveTowards(transform.position, target.position, speed * Time.fixedDeltaTime);
        rig.MovePosition(pos);
        // print(speed);
    }

    private void OnTriggerEnter(Collider col)
    {
        // print(col);
        if(col.GetComponent<Collider>().tag == "HurtTag1")
        {
            health -= 10;
            healthBar.fillAmount -= healthFillAmount;
            // if (globalGameState.GetDifficulty() == 0)
            // {
            //     healthBar.fillAmount -= 0.3333333333333333f;
            // } else if (globalGameState.GetDifficulty() == 1)
            // {
            //     healthBar.fillAmount -= 0.1666666666666667f;
            // } else if (globalGameState.GetDifficulty() == 2)
            // {
            //     healthBar.fillAmount -= 0.1f;
            // }

        }
    }

    IEnumerator SpeedLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(15f);
            speed = newSpeed;
            yield return new WaitForSeconds(8f);
            speed = baseSpeed;
        }
    }
}