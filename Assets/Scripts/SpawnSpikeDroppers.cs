using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSpikeDroppers : MonoBehaviour
{
    [SerializeField] private GameObject SpikeSpawner;
    [SerializeField] private GameObject spikes;
    [SerializeField] private bool flag = false;
    void Start()
    {
        StartCoroutine(Wait()); 
    }

    // Update is called once per frame
    void Update()
    {

    }


    public IEnumerator Wait()
    {
        while (!flag)
        {
            yield return new WaitForSeconds(0.4f);
            int x = Random.Range(-15, 15);
            int z = Random.Range(-15, 15);
            Vector3 vector = new Vector3(x,15,z);
            spikes = (GameObject) Instantiate(SpikeSpawner, vector, Quaternion.Euler(vector));
            Destroy(spikes, 5.5f);
        }
    }
}