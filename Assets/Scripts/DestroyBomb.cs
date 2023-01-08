using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyBomb : MonoBehaviour
{
    private PickupItem itemScript;

    void Start()
    {
        StartCoroutine(Wait());
        itemScript = GetComponent<PickupItem>();
    }

    IEnumerator Wait()
    {
        while(true)
        {
            yield return new WaitForSeconds(10);
            if(itemScript.itemPickedUp == false)
            {
                Destroy(gameObject);
            }
        }
    }
}
