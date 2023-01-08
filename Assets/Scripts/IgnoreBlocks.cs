using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IgnoreBlocks : MonoBehaviour
{
    [SerializeField] private GameObject block1;
    [SerializeField] private GameObject block2;

    private void Start()
    {
        Physics.IgnoreCollision(block1.GetComponent<Collider>(), GetComponent<Collider>());
        Physics.IgnoreCollision(block2.GetComponent<Collider>(), GetComponent<Collider>());
    }
}
