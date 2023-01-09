using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public GameObject attackRing;
    public GameObject attackPosition;
    public GameObject enemy;
    private float magnitude = 15f;
    private bool attack = false;

    void Start()
    {
        StartCoroutine(AttackLoop());
    }

    private void OnTriggerEnter(Collider col)
    {
        if(col.tag == "Ground")
        {
            print("Ground");
            if(attack)
            {
                print("Attack");
                Instantiate(attackRing, attackPosition.transform.position, attackPosition.transform.rotation);
                enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * magnitude, ForceMode.Impulse);
            }
        }
    }

    IEnumerator AttackLoop()
    {
        while (true)
        {
            yield return new WaitForSeconds(5f);
            attack = false;
            enemy.GetComponent<BossEnemy>().enabled = true;
            yield return new WaitForSeconds(5f);
            enemy.GetComponent<BossEnemy>().enabled = false;
            attack = true;
            enemy.GetComponent<Rigidbody>().AddForce(Vector3.up * magnitude, ForceMode.Impulse);
        }
    }
}
