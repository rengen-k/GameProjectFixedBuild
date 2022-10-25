using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemRespawn : MonoBehaviour
{
    public GameObject itemPrefab;
    public Transform player;
    Bomb bombScript;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        bombScript = itemPrefab.GetComponent<Bomb>();
    }

    void Update()
    {
        if (Vector3.Distance(transform.position, player.position) < 2 && bombScript.hasExploded)
        {
            SpawnItem();
        }
    }

    void SpawnItem()
    {
        GameObject item = Instantiate(itemPrefab, transform.position, transform.rotation);
    }
}
