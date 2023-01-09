using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnBombs : MonoBehaviour
{

    [SerializeField] private GameObject Bomb;

    void Start()
    {
        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while(true)
        {
            yield return new WaitForSeconds(5);
            print("SPAWN");
            var position = new Vector3(Random.Range(-30f, 0f), 10, Random.Range(20f, 40f));
            Instantiate(Bomb, position, Quaternion.identity);
        }
    }
}
