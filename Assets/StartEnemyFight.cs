using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartEnemyFight : MonoBehaviour
{
    public GameObject puzzle;
    public GameObject cube;
    public GameObject enemy;

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Player")
        {
            if(GetComponent<BoxCollider>().isTrigger)
            {
                puzzle.GetComponent<Animator>().SetTrigger("Roll");
                cube.GetComponent<Animator>().SetTrigger("Roll");
                enemy.SetActive(true);
            }
        }
    }
}
