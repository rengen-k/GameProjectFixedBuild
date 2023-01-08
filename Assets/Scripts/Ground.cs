using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ground : MonoBehaviour
{
    public GameObject attackRing;
    public GameObject attackPosition;
    
    private void OnTriggerEnter(Collider col)
    {
        Instantiate(attackRing, attackPosition.transform.position, attackPosition.transform.rotation);
    }
}
