using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHead : MonoBehaviour
{
    private EnemyAI enemyAIScript;

    private void onEnable()
    {
        enemyAIScript = new EnemyAI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            enemyAIScript.Respawn();
        }
    }
}
