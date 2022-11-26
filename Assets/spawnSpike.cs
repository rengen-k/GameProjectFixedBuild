using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnSpike : MonoBehaviour
{
    [SerializeField] GameObject spike;
    [SerializeField] int[] interval = {1};
    private float timeToDrop;
    private int index;
    // Start is called before the first frame update
    void Start()
    {

        timeToDrop = interval[0];
        index = 1% interval.Length;
    }

    // Update is called once per frame
    void Update()
    {
        timeToDrop += Time.deltaTime;
        if (timeToDrop >= interval[index])
        {
            timeToDrop = 0;
            index = (index+1) % interval.Length;

            GameObject temp = Instantiate(spike, transform.position, transform.rotation);
            temp.AddComponent<Rigidbody>();
            StartCoroutine(destroyTemp(temp));
        }
    }

    IEnumerator destroyTemp(GameObject temp)
    {
        yield return new WaitForSeconds(5f);
        Destroy(temp);
    }
}
