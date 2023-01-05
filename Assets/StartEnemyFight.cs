using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemyFight : MonoBehaviour
{
    public GameObject puzzle;
    public GameObject cube;
    public GameObject enemy;


    [SerializeField] private GameObject player;

    [SerializeField] private GameObject wall1;
    [SerializeField] private GameObject wall2;
    [SerializeField] private GameObject wall3;
    [SerializeField] private GameObject wall4;

    [SerializeField] private GameObject healthBar;

    [SerializeField] private GameObject spawnBombs;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(GetComponent<BoxCollider>().isTrigger)
            {
                wall1.GetComponent<BoxCollider>().enabled = true;
                wall2.GetComponent<BoxCollider>().enabled = true;
                wall3.GetComponent<BoxCollider>().enabled = true;
                wall4.GetComponent<BoxCollider>().enabled = true;

                healthBar.SetActive(true);

                puzzle.GetComponent<Animator>().SetTrigger("Roll");
                cube.GetComponent<Animator>().SetTrigger("Roll");
                enemy.SetActive(true);
                spawnBombs.GetComponent<SpawnBombs>().enabled = true;
                player.GetComponent<PlayerController>().lastGroundedPosition = new Vector3(gameObject.transform.position.x, gameObject.transform.position.y + 0.3f, gameObject.transform.position.z);
                Destroy(gameObject);
            }
        }
    }
}
