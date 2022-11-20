using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MeteorManager : MonoBehaviour
{
    //TODO, make drop background meteors 
    [Tooltip("The meteor target this manager spawns")]
    [SerializeField] private GameObject target;
    private bool allow = false;
    [SerializeField] private int maxMeteors;
    private int meteorCount = 0;
    private Transform player;
    int i = 0;



    void Start()
    {
        player = GameObject.Find("Player").transform;
        StartCoroutine(Wait());
    }

    void Update()
    {
        if (allow && Random.Range(0,100) < 2)
        {   
            StartCoroutine(Drop());
        }
        if (meteorCount >= maxMeteors)
        {
            allow = false;
        }
        if ((i++ % 25 == 0))
        {
            BackgroundDrop();
        }

    }

    void BackgroundDrop()
    {
        

    }



    IEnumerator Drop()
    {
        meteorCount++;
        //Generate random posistion on navmesh, spawn Meteor Target, yield return new waitforseconds(delay).
        Vector3 randomDirection = Random.insideUnitSphere * 40;
        randomDirection += player.position;
        NavMeshHit hit;
        Vector3 finalPosition = Vector3.zero;
        if (NavMesh.SamplePosition(randomDirection, out hit, 40, 1)) {
            finalPosition = hit.position;            
        }
        float delay = Random.Range(0.5f, 5f);
        Instantiate(target, finalPosition, transform.rotation).GetComponent<MeteorTarget>().SetDelay(delay);
        yield return new WaitForSeconds(delay+2);
        meteorCount--;
        allow = true;
    }

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(10);
        allow = true;
    }

    
}
