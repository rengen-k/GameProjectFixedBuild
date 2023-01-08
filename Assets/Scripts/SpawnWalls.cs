using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnWalls : MonoBehaviour
{
    [SerializeField] private GameObject walls;
    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(GetComponent<BoxCollider>().isTrigger)
            {
                walls.SetActive(true);
            }
        }
    }
}
